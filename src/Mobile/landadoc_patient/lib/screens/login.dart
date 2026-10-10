import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../api.dart';
import '../l10n.dart';
import '../session.dart';
import '../widgets.dart';
import 'register.dart';

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
      setState(() => _error = e.offline ? l.t('networkError') : l.t('wrongLogin'));
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    // White like the website's login card, so the logo (white background) blends in
    return Scaffold(
      backgroundColor: Colors.white,
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
                    onFieldSubmitted: (_) => _submit(),   // Enter logs in, like the web forms
                    decoration: InputDecoration(labelText: l.t('password')),
                    validator: (v) => (v ?? '').isEmpty ? l.t('required') : null,
                  ),
                  if (_error != null) ...[const SizedBox(height: 12), ErrorBanner(_error!)],
                  const SizedBox(height: 20),
                  FilledButton(onPressed: _busy ? null : _submit, child: _busy ? const ButtonSpinner() : Text(l.t('logIn'))),
                  const SizedBox(height: 12),
                  TextButton(
                    onPressed: () => Navigator.of(context).push(MaterialPageRoute(builder: (_) => const RegisterScreen())),
                    child: Text('${l.t('noAccount')} ${l.t('register')}'),
                  ),
                  const SizedBox(height: 8),
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
