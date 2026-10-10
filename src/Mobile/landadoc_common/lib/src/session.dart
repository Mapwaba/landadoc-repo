import 'dart:convert';

import 'package:flutter/foundation.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'api.dart';
import 'models.dart';

// The signed-in user. Tokens live in the phone's secure storage (Keystore on Android), so they
// stay signed in between launches until they log out or the refresh token (7 days) runs out.
// Each app takes one kind of account: role "Patient" or "Doctor".
class Session extends ChangeNotifier {
  static const _accessKey = 'landadoc_access_token';
  static const _refreshKey = 'landadoc_refresh_token';

  final String role;
  // Where the app keeps its last appointments list for offline reading
  String get appointmentsCacheKey => 'landadoc_appointments_${role.toLowerCase()}';

  final Api api = Api();
  final FlutterSecureStorage _storage = const FlutterSecureStorage();
  UserProfile? user;
  bool loading = true;

  bool get signedIn => api.accessToken != null;

  // Country from the token (e.g. "CD"): in the DRC, card isn't offered (Mobile Money or insurer)
  String? get country => _claim('http://schemas.xmlsoap.org/ws/2005/05/identity/claims/country') ?? user?.country;
  String? get userId => _claim('http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier');

  Session({required this.role}) {
    api.onTokens = _saveTokens;
    api.onSignedOut = logout;
  }

  Future<void> restore() async {
    api.accessToken = await _storage.read(key: _accessKey);
    api.refreshToken = await _storage.read(key: _refreshKey);
    loading = false;
    notifyListeners();
    if (signedIn) {
      try {
        user = await api.me();
        notifyListeners();
      } catch (_) {
        // offline: stay signed in with what we have; the profile loads next time
      }
    }
  }

  // Throws ApiException; an account of another kind is refused here (403, "wrong-role")
  Future<void> login(String email, String password) => _start(api.login(email.trim(), password));

  Future<void> register(Map<String, dynamic> request) => _start(api.register(request));

  Future<void> _start(Future<Map<String, dynamic>> call) async {
    final auth = await call;
    final profile = auth['user'] is Map<String, dynamic> ? UserProfile.fromJson(auth['user'] as Map<String, dynamic>) : null;
    if (profile != null && profile.role != role) throw ApiException(403, 'wrong-role', 'wrong-role');
    api.accessToken = auth['token'] as String;
    api.refreshToken = auth['refreshToken'] as String;
    await _saveTokens(api.accessToken!, api.refreshToken!);
    user = profile ?? await api.me();
    notifyListeners();
  }

  Future<void> logout() async {
    api.accessToken = null;
    api.refreshToken = null;
    user = null;
    await _storage.delete(key: _accessKey);
    await _storage.delete(key: _refreshKey);
    final prefs = await SharedPreferences.getInstance();
    await prefs.remove(appointmentsCacheKey);
    notifyListeners();
  }

  Future<void> _saveTokens(String access, String refresh) async {
    await _storage.write(key: _accessKey, value: access);
    await _storage.write(key: _refreshKey, value: refresh);
  }

  // A claim from the access token's payload (not verified here; the services do that)
  String? _claim(String type) {
    final token = api.accessToken;
    if (token == null) return null;
    try {
      final payload = token.split('.')[1];
      final json = utf8.decode(base64Url.decode(base64Url.normalize(payload)));
      return (jsonDecode(json) as Map<String, dynamic>)[type]?.toString();
    } catch (_) {
      return null;
    }
  }
}
