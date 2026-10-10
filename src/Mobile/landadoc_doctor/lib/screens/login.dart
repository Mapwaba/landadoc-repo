import 'package:flutter/material.dart';
import 'package:landadoc_common/landadoc_common.dart';
import 'package:provider/provider.dart';
import 'package:url_launcher/url_launcher.dart';

// Doctors sign in here; "Create an account" opens the website's registration, where new doctors
// also upload their ID and membership card for approval.
class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  State<LoginScreen> createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final _form = GlobalKey<FormState>();
  final _email = TextEditingController();
  final _password = TextEditingController();
  bool _busy = false;
  String? _error;

  @override
  void dispose() {
    _email.dispose();
    _password.dispose();
    super.dispose();
  }

  Future<void> _submit() async {
    final l = context.read<L10n>();
    if (_busy || !_form.currentState!.validate()) return;
    setState(() {
      _busy = true;
      _error = null;
    });
    try {
      await context.read<Session>().login(_email.text, _password.text);
    } on ApiException catch (e) {
      setState(() => _error = e.offline ? l.t('networkError') : e.code == 'wrong-role' ? l.t('doctorOnly') : l.t('wrongLogin'));
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    return Scaffold(
      backgroundColor: Colors.white,   // the logo has a white background
      body: SafeArea(
        child: Center(
          child: SingleChildScrollView(
            padding: const EdgeInsets.all(24),
            child: Form(
              key: _form,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: [
                  const BrandHeader(),
                  const SizedBox(height: 32),
                  TextFormField(
                    controller: _email,
                    keyboardType: TextInputType.emailAddress,
                    autofillHints: const [AutofillHints.email],
                    textInputAction: TextInputAction.next,
                    decoration: InputDecoration(labelText: l.t('email')),
                    validator: (v) => (v ?? '').trim().isEmpty ? l.t('required') : null,
                  ),
                  const SizedBox(height: 12),
                  TextFormField(
                    controller: _password,
                    obscureText: true,
                    autofillHints: const [AutofillHints.password],
                    textInputAction: TextInputAction.done,
                    onFieldSubmitted: (_) => _submit(),
                    decoration: InputDecoration(labelText: l.t('password')),
                    validator: (v) => (v ?? '').isEmpty ? l.t('required') : null,
                  ),
                  if (_error != null) ...[const SizedBox(height: 12), ErrorBanner(_error!)],
                  const SizedBox(height: 20),
                  FilledButton(onPressed: _busy ? null : _submit, child: _busy ? const ButtonSpinner() : Text(l.t('logIn'))),
                  const SizedBox(height: 12),
                  // Registering needs a photo, specialty, licence, clinics and ID documents for approval:
                  // that's the website's registration page, opened in the browser
                  TextButton(
                    onPressed: () => launchUrl(Uri.parse('${ApiConfig.doctorWebsite}/register'), mode: LaunchMode.externalApplication),
                    child: Text('${l.t('noAccount')} ${l.t('register')}', textAlign: TextAlign.center),
                  ),
                  Text(l.t('registerOnWeb'), textAlign: TextAlign.center, style: TextStyle(color: Colors.grey[700], fontSize: 13)),
                  const SizedBox(height: 16),
                  const LanguageSwitch(),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
