import 'dart:async';
import 'dart:convert';
import 'dart:io';

import 'package:http/http.dart' as http;

import 'config.dart';
import 'models.dart';

// A failed request: Status is the HTTP status (0 when LandaDoc couldn't be reached) and Message
// the service's own "error" text when it gave one.
class ApiException implements Exception {
  final int status;
  final String? message;
  final String? code;

  ApiException(this.status, [this.message, this.code]);

  bool get offline => status == 0;

  @override
  String toString() => 'ApiException($status, $message)';
}

// Talks to the LandaDoc services. Every call carries the access token; when it has expired
// (401), the refresh token gets a new pair once and the call is retried. If that fails too,
// onSignedOut runs so the app returns to the login screen.
class Api {
  final http.Client _http = http.Client();
  String? accessToken;
  String? refreshToken;
  Future<void> Function(String access, String refresh)? onTokens;
  Future<void> Function()? onSignedOut;
  Future<bool>? _refreshing;

  // ── Accounts ───────────────────────────────────────────────────────
  Future<Map<String, dynamic>> login(String email, String password) async =>
      await _send('POST', ApiConfig.identity, 'api/auth/login', body: {'email': email, 'password': password}, auth: false) as Map<String, dynamic>;

  Future<Map<String, dynamic>> register(Map<String, dynamic> request) async =>
      await _send('POST', ApiConfig.identity, 'api/auth/register', body: request, auth: false) as Map<String, dynamic>;

  Future<UserProfile> me() async => UserProfile.fromJson(await _send('GET', ApiConfig.identity, 'api/auth/me') as Map<String, dynamic>);

  Future<List<FamilyPerson>> family() async {
    try {
      return FamilyPerson.fromOverview(await _send('GET', ApiConfig.identity, 'api/family/mine') as Map<String, dynamic>);
    } on ApiException catch (e) {
      if (e.status == 404) return [];
      rethrow;
    }
  }

  // ── Doctors & times ────────────────────────────────────────────────
  // photos=false: no inline photos, they're loaded (and cached) from photoUrl instead
  Future<List<Doctor>> searchDoctors(String query) async {
    final q = Uri(queryParameters: {'q': query, 'photos': 'false'}).query;
    final list = await _send('GET', ApiConfig.search, 'api/search/doctors?$q', auth: false) as List;
    return list.map((d) => Doctor.fromJson(d as Map<String, dynamic>)).toList();
  }

  Future<Doctor> doctor(String id) async =>
      Doctor.fromJson(await _send('GET', ApiConfig.search, 'api/search/doctors/$id?photos=false', auth: false) as Map<String, dynamic>);

  static String photoUrl(String doctorId) => '${ApiConfig.search}api/search/doctors/$doctorId/photo';

  Future<SlotRange> slots(String doctorId, DateTime from, int days) async {
    final date = '${from.year.toString().padLeft(4, '0')}-${from.month.toString().padLeft(2, '0')}-${from.day.toString().padLeft(2, '0')}';
    return SlotRange.fromJson(
        await _send('GET', ApiConfig.availability, 'api/availability/slots/range?doctorId=$doctorId&from=$date&days=$days', auth: false) as Map<String, dynamic>);
  }

  // ── Appointments ───────────────────────────────────────────────────
  // slotStart is the doctor's wall-clock time, sent with "Z" like the web apps do
  Future<Appointment> book(String doctorId, DateTime slotStart, String? motif, String? forPatientId) async {
    final iso = '${slotStart.toIso8601String().split('.').first}Z';
    return Appointment.fromJson(await _send('POST', ApiConfig.appointment, 'api/appointments',
        body: {'doctorId': doctorId, 'slotStart': iso, 'motif': motif, 'bookForPatientId': forPatientId}) as Map<String, dynamic>);
  }

  Future<String> myAppointmentsRaw() async => jsonEncode(await _send('GET', ApiConfig.appointment, 'api/appointments/mine'));

  static List<Appointment> parseAppointments(String json) =>
      (jsonDecode(json) as List).map((a) => Appointment.fromJson(a as Map<String, dynamic>)).toList();

  Future<Appointment> appointment(String id) async =>
      Appointment.fromJson(await _send('GET', ApiConfig.appointment, 'api/appointments/$id') as Map<String, dynamic>);

  // ── Payment ────────────────────────────────────────────────────────
  Future<Payment?> paymentFor(String appointmentId) async {
    try {
      return Payment.fromJson(await _send('GET', ApiConfig.payment, 'api/payments/by-appointment/$appointmentId') as Map<String, dynamic>);
    } on ApiException catch (e) {
      if (e.status == 404) return null;
      rethrow;
    }
  }

  // provider: "MokoAfrika" (phone, operator, fullName) or "Stripe" (answer has a checkoutUrl)
  Future<Map<String, dynamic>> pay(String paymentId, Map<String, dynamic> request) async =>
      await _send('POST', ApiConfig.payment, 'api/payments/$paymentId/initiate', body: request) as Map<String, dynamic>;

  // ── Plumbing ───────────────────────────────────────────────────────
  Future<Object?> _send(String method, String base, String path, {Object? body, bool auth = true, bool retried = false}) async {
    final request = http.Request(method, Uri.parse('$base$path'));
    request.headers['Accept'] = 'application/json';
    if (body != null) {
      request.headers['Content-Type'] = 'application/json';
      request.body = jsonEncode(body);
    }
    if (auth && accessToken != null) request.headers['Authorization'] = 'Bearer $accessToken';

    http.Response response;
    try {
      response = await http.Response.fromStream(await _http.send(request).timeout(ApiConfig.timeout));
    } on SocketException {
      throw ApiException(0);
    } on TimeoutException {
      throw ApiException(0);
    } on http.ClientException {
      throw ApiException(0);
    }

    if (response.statusCode == 401 && auth && !retried && refreshToken != null) {
      if (await _refresh()) return _send(method, base, path, body: body, auth: auth, retried: true);
      await onSignedOut?.call();
    }
    if (response.statusCode >= 200 && response.statusCode < 300) {
      return response.body.isEmpty ? null : jsonDecode(utf8.decode(response.bodyBytes));
    }
    throw _errorOf(response);
  }

  // One refresh at a time, even when several calls hit 401 together
  Future<bool> _refresh() => _refreshing ??= () async {
        try {
          final response = await _http
              .post(Uri.parse('${ApiConfig.identity}api/auth/refresh'),
                  headers: {'Content-Type': 'application/json'}, body: jsonEncode({'refreshToken': refreshToken}))
              .timeout(ApiConfig.timeout);
          if (response.statusCode != 200) return false;
          final auth = jsonDecode(response.body) as Map<String, dynamic>;
          accessToken = auth['token'] as String;
          refreshToken = auth['refreshToken'] as String;
          await onTokens?.call(accessToken!, refreshToken!);
          return true;
        } catch (_) {
          return false;
        } finally {
          _refreshing = null;
        }
      }();

  static ApiException _errorOf(http.Response response) {
    try {
      final body = jsonDecode(utf8.decode(response.bodyBytes));
      if (body is Map<String, dynamic>) {
        final message = (body['error'] ?? body['detail'] ?? body['title'])?.toString();
        return ApiException(response.statusCode, message, body['code']?.toString());
      }
    } catch (_) {}
    return ApiException(response.statusCode);
  }
}
