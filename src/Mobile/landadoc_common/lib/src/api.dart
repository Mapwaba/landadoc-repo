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

  Future<SlotRange> slots(String doctorId, DateTime from, int days) async => SlotRange.fromJson(
      await _send('GET', ApiConfig.availability, 'api/availability/slots/range?doctorId=$doctorId&from=${dateParam(from)}&days=$days', auth: false)
          as Map<String, dynamic>);

  // ── Appointments ───────────────────────────────────────────────────
  // slotStart is the doctor's wall-clock time, sent with "Z" like the web apps do
  Future<Appointment> book(String doctorId, DateTime slotStart, String? motif, String? forPatientId) async {
    return Appointment.fromJson(await _send('POST', ApiConfig.appointment, 'api/appointments',
        body: {'doctorId': doctorId, 'slotStart': wallParam(slotStart), 'motif': motif, 'bookForPatientId': forPatientId}) as Map<String, dynamic>);
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

  // ── Doctor app ─────────────────────────────────────────────────────
  // The doctor's own profile (approval status); null when they haven't created one yet
  Future<Map<String, dynamic>?> myDoctorProfile() async {
    try {
      return await _send('GET', ApiConfig.admin, 'api/doctors/me') as Map<String, dynamic>;
    } on ApiException catch (e) {
      if (e.status == 404) return null;
      rethrow;
    }
  }

  // The doctor's appointments with their patients' names filled in (the Appointment service
  // only knows ids), as JSON so the app can keep a copy for offline reading
  Future<String> doctorAgendaRaw() async {
    final results = await Future.wait([
      _send('GET', ApiConfig.appointment, 'api/appointments/mine'),
      _send('GET', ApiConfig.identity, 'api/patients/mine/names').catchError((_) => <dynamic>[]),
    ]);
    final names = {
      for (final n in (results[1] as List? ?? [])) n['id'] as String: '${n['firstName']} ${n['lastName']}'.trim(),
    };
    final appointments = (results[0] as List).cast<Map<String, dynamic>>();
    for (final a in appointments) {
      a['patientName'] ??= names[a['patientId']];
    }
    return jsonEncode(appointments);
  }

  Future<void> completeAppointment(String id) async => _send('PATCH', ApiConfig.appointment, 'api/appointments/$id/complete');

  Future<List<PatientContact>> myPatients() async =>
      ((await _send('GET', ApiConfig.identity, 'api/patients/mine')) as List).map((p) => PatientContact.fromJson(p as Map<String, dynamic>)).toList();

  Future<List<MedicalDocument>> documentsOf(String patientId) async =>
      ((await _send('GET', ApiConfig.document, 'api/documents?patientId=$patientId')) as List)
          .map((d) => MedicalDocument.fromJson(d as Map<String, dynamic>))
          .toList();

  // A short-lived link to open the file
  Future<String?> documentUrl(String documentId) async =>
      ((await _send('GET', ApiConfig.document, 'api/documents/$documentId/download-url')) as Map<String, dynamic>)['url'] as String?;

  Future<List<InsuranceClaim>> myClaims() async =>
      ((await _send('GET', ApiConfig.payment, 'api/insurance-claims/doctor/me')) as List)
          .map((c) => InsuranceClaim.fromJson(c as Map<String, dynamic>))
          .toList();

  // Approving needs the reference the insurer gave when the doctor checked the cover
  Future<void> approveClaim(String id, String reference) async =>
      _send('POST', ApiConfig.payment, 'api/insurance-claims/$id/approve', body: {'note': reference});

  Future<void> declineClaim(String id, String? reason) async =>
      _send('POST', ApiConfig.payment, 'api/insurance-claims/$id/decline', body: {'note': reason});

  Future<List<ScheduleDay>> mySchedule() async =>
      ((await _send('GET', ApiConfig.availability, 'api/availability/schedule/mine')) as List)
          .map((d) => ScheduleDay.fromJson(d as Map<String, dynamic>))
          .toList();

  // Slots the doctor took out of their schedule, as wall-clock times
  Future<List<DateTime>> myBlocked(DateTime from, int days) async =>
      ((await _send('GET', ApiConfig.availability, 'api/availability/blocked/mine?from=${dateParam(from)}&days=$days')) as List)
          .map((b) => wallClock(b['slotStart'] as String))
          .toList();

  Future<void> block(List<DateTime> slots, {String? reason}) async =>
      _send('POST', ApiConfig.availability, 'api/availability/blocked/mine', body: {'slotStarts': slots.map(wallParam).toList(), 'reason': reason});

  Future<void> unblock(List<DateTime> slots) async =>
      _send('POST', ApiConfig.availability, 'api/availability/blocked/mine/unblock', body: {'slotStarts': slots.map(wallParam).toList()});

  // A date for a query string, and a wall-clock time as the services expect it ("…T09:30:00Z")
  static String dateParam(DateTime d) =>
      '${d.year.toString().padLeft(4, '0')}-${d.month.toString().padLeft(2, '0')}-${d.day.toString().padLeft(2, '0')}';
  static String wallParam(DateTime t) =>
      '${dateParam(t)}T${t.hour.toString().padLeft(2, '0')}:${t.minute.toString().padLeft(2, '0')}:00Z';

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
