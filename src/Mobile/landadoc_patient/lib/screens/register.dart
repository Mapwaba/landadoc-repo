import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:landadoc_common/landadoc_common.dart';


// The essentials of the web registration: name, email, phone, password, ID number and country
// (DR Congo or elsewhere — it decides which payment methods are offered). Address details and a
// photo can be added later on the website.
class RegisterScreen extends StatefulWidget {
  const RegisterScreen({super.key});

  @override
  State<RegisterScreen> createState() => _RegisterScreenState();
}

class _RegisterScreenState extends State<RegisterScreen> {
  final _form = GlobalKey<FormState>();
  final _first = TextEditingController();
  final _last = TextEditingController();
  final _email = TextEditingController();
  final _phone = TextEditingController();
  final _idNumber = TextEditingController();
  final _password = TextEditingController();
  final _confirm = TextEditingController();
  String _country = 'CD';
  String _otherCountry = '';
  bool _busy = false;
  String? _error;

  @override
  void dispose() {
    for (final c in [_first, _last, _email, _phone, _idNumber, _password, _confirm]) {
      c.dispose();
    }
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
      await context.read<Session>().register({
        'email': _email.text.trim(),
        'password': _password.text,
        'firstName': _first.text.trim(),
        'lastName': _last.text.trim(),
        'phone': _phone.text.trim(),
        'country': _country == 'CD' ? 'CD' : _otherCountry.trim().toUpperCase(),
        'idNumber': _idNumber.text.trim(),
      });
      if (mounted) Navigator.of(context).popUntil((r) => r.isFirst);
    } on ApiException catch (e) {
      setState(() => _error = e.offline ? l.t('networkError') : (e.message ?? l.t('registerFailed')));
    } finally {
      if (mounted) setState(() => _busy = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    String? required(String? v) => (v ?? '').trim().isEmpty ? l.t('required') : null;
    Widget field(TextEditingController c, String label, {TextInputType? type, bool obscure = false, String? Function(String?)? validator, bool last = false}) =>
        Padding(
          padding: const EdgeInsets.only(bottom: 12),
          child: TextFormField(
            controller: c,
            keyboardType: type,
            obscureText: obscure,
            textInputAction: last ? TextInputAction.done : TextInputAction.next,
            onFieldSubmitted: last ? (_) => _submit() : null,
            decoration: InputDecoration(labelText: label),
            validator: validator ?? required,
          ),
        );

    return Scaffold(
      backgroundColor: Colors.white,   // the logo has a white background
      appBar: AppBar(title: Text(l.t('register'))),
      body: SafeArea(
        child: SingleChildScrollView(
          padding: const EdgeInsets.all(20),
          child: Form(
            key: _form,
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: [
                const BrandHeader(width: 200),
                const SizedBox(height: 24),
                field(_first, l.t('firstName')),
                field(_last, l.t('lastName')),
                field(_email, l.t('email'), type: TextInputType.emailAddress),
                field(_phone, l.t('phone'), type: TextInputType.phone),
                field(_idNumber, l.t('idNumber')),
                Padding(
                  padding: const EdgeInsets.only(bottom: 12),
                  child: DropdownButtonFormField<String>(
                    initialValue: _country,
                    decoration: InputDecoration(labelText: l.t('country')),
                    items: [
                      DropdownMenuItem(value: 'CD', child: Text(l.t('countryDrc'))),
                      DropdownMenuItem(value: 'other', child: Text(l.t('countryOther'))),
                    ],
                    onChanged: (v) => setState(() => _country = v ?? 'CD'),
                  ),
                ),
                if (_country != 'CD')
                  Padding(
                    padding: const EdgeInsets.only(bottom: 12),
                    child: TextFormField(
                      maxLength: 2,
                      textCapitalization: TextCapitalization.characters,
                      decoration: const InputDecoration(labelText: 'ISO (FR, BE, US…)'),
                      onChanged: (v) => _otherCountry = v,
                      validator: (v) => (v ?? '').trim().length == 2 ? null : l.t('required'),
                    ),
                  ),
                field(_password, l.t('password'), obscure: true,
                    validator: (v) => (v ?? '').length < 8 ? l.t('passwordTooShort') : null),
                field(_confirm, l.t('confirmPassword'), obscure: true, last: true,
                    validator: (v) => v == _password.text ? null : l.t('passwordsDiffer')),
                if (_error != null) ...[ErrorBanner(_error!), const SizedBox(height: 12)],
                FilledButton(onPressed: _busy ? null : _submit, child: _busy ? const ButtonSpinner() : Text(l.t('register'))),
                TextButton(onPressed: () => Navigator.of(context).pop(), child: Text('${l.t('haveAccount')} ${l.t('logIn')}')),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
