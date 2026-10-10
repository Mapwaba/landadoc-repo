import 'dart:async';

import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:url_launcher/url_launcher.dart';
import 'package:landadoc_common/landadoc_common.dart';


// One appointment and its payment. Mobile Money: the patient gets a prompt on their phone; this
// screen checks every few seconds until the payment is recorded (or fails). Card (outside the
// DRC only) opens the secure card page in the browser. Insurance stays on the website for now.
class AppointmentScreen extends StatefulWidget {
  final String appointmentId;
  const AppointmentScreen(this.appointmentId, {super.key});

  @override
  State<AppointmentScreen> createState() => _AppointmentScreenState();
}

enum _Operator { airtel, orange, mpesa, africell }

class _AppointmentScreenState extends State<AppointmentScreen> {
  Appointment? _appointment;
  Payment? _payment;
  String? _loadError;
  Timer? _poll;

  final _name = TextEditingController();
  final _phone = TextEditingController();
  _Operator _operator = _Operator.mpesa;
  bool _paying = false;
  bool _waiting = false;   // the prompt is on the patient's phone
  String? _payError;

  @override
  void initState() {
    super.initState();
    final user = context.read<Session>().user;
    _name.text = user?.fullName ?? '';
    _phone.text = user?.phone ?? '';
    _load();
  }

  @override
  void dispose() {
    _poll?.cancel();
    _name.dispose();
    _phone.dispose();
    super.dispose();
  }

  Future<void> _load() async {
    final api = context.read<Session>().api;
    final l = context.read<L10n>();
    try {
      final results = await Future.wait([api.appointment(widget.appointmentId), api.paymentFor(widget.appointmentId)]);
      if (!mounted) return;
      setState(() {
        _appointment = results[0] as Appointment;
        _payment = results[1] as Payment?;
        _loadError = null;
      });
    } on ApiException catch (e) {
      if (mounted) setState(() => _loadError = e.offline ? l.t('networkError') : (e.message ?? l.t('networkError')));
    }
  }

  Future<void> _payMobileMoney() async {
    final payment = _payment;
    if (payment == null || _paying) return;
    final l = context.read<L10n>();
    if (_name.text.trim().isEmpty || _phone.text.trim().isEmpty) {
      setState(() => _payError = l.t('required'));
      return;
    }
    setState(() {
      _paying = true;
      _payError = null;
    });
    try {
      final operatorName = switch (_operator) {
        _Operator.airtel => 'Airtel',
        _Operator.orange => 'Orange',
        _Operator.mpesa => 'Mpesa',
        _Operator.africell => 'Africell',
      };
      await context.read<Session>().api.pay(payment.id, {
        'provider': 'MokoAfrika',
        'phoneNumber': _phone.text.trim(),
        'operator': operatorName,
        'patientFullName': _name.text.trim(),
      });
      setState(() => _waiting = true);
      _startPolling();
    } on ApiException catch (e) {
      setState(() => _payError = e.offline ? l.t('networkError') : '${l.t('payFailed')}${e.message != null ? ' (${e.message})' : ''}');
    } finally {
      if (mounted) setState(() => _paying = false);
    }
  }

  Future<void> _payByCard() async {
    final payment = _payment;
    if (payment == null || _paying) return;
    final l = context.read<L10n>();
    setState(() {
      _paying = true;
      _payError = null;
    });
    try {
      final answer = await context.read<Session>().api.pay(payment.id, {'provider': 'Stripe'});
      final url = answer['checkoutUrl'] as String?;
      if (url != null) {
        await launchUrl(Uri.parse(url), mode: LaunchMode.externalApplication);
        _startPolling();
      }
    } on ApiException catch (e) {
      setState(() => _payError = e.offline ? l.t('networkError') : '${l.t('payFailed')}${e.message != null ? ' (${e.message})' : ''}');
    } finally {
      if (mounted) setState(() => _paying = false);
    }
  }

  // Every 4 s for up to 3 min, until the payment is no longer pending
  void _startPolling() {
    _poll?.cancel();
    var ticks = 0;
    _poll = Timer.periodic(const Duration(seconds: 4), (timer) async {
      ticks++;
      try {
        final payment = await context.read<Session>().api.paymentFor(widget.appointmentId);
        if (!mounted) return;
        if (payment != null && payment.status != PaymentStatus.pending) {
          timer.cancel();
          await _load();
          if (!mounted) return;
          setState(() {
            _waiting = false;
            if (payment.status == PaymentStatus.failed) _payError = context.read<L10n>().t('paymentFailedRetry');
          });
        }
      } catch (_) {
        // a missed check is fine; the next one tries again
      }
      if (ticks >= 45) {
        timer.cancel();
        if (mounted) setState(() => _waiting = false);
      }
    });
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final a = _appointment;
    final p = _payment;
    final session = context.watch<Session>();
    final inDrc = (session.country ?? '').toUpperCase() == 'CD';
    final format = DateFormat('EEEE d MMMM y · HH:mm', l.language);

    return Scaffold(
      appBar: AppBar(title: Text(a == null ? l.t('appointment') : l.f('ref', [a.refNumber]))),
      body: _loadError != null && a == null
          ? RetryPanel(_loadError!, _load)
          : a == null
              ? const Center(child: CircularProgressIndicator())
              : RefreshIndicator(
                  onRefresh: _load,
                  child: ListView(padding: const EdgeInsets.all(16), children: [
                    Row(children: [
                      Expanded(child: Text(a.doctorName ?? '—', style: Theme.of(context).textTheme.titleLarge)),
                      AppointmentStatusChip(a.status, l.t(a.statusKey)),
                    ]),
                    if (a.specialty != null) Text(a.specialty!, style: TextStyle(color: Theme.of(context).colorScheme.primary)),
                    const SizedBox(height: 8),
                    _line(Icons.event, format.format(a.slotStart)),
                    if (a.clinicName != null) _line(Icons.location_on_outlined, a.clinicName!),
                    if (a.patientId != session.userId && a.patientName != null) _line(Icons.person_outline, l.f('for', [a.patientName!])),
                    if ((a.motif ?? '').isNotEmpty) _line(Icons.notes, a.motif!),
                    if (p != null) ...[
                      const Divider(height: 32),
                      Row(children: [
                        Text(l.t('payment'), style: Theme.of(context).textTheme.titleMedium),
                        const Spacer(),
                        Text('${money(p.gross)} · ${l.t(p.statusKey)}', style: const TextStyle(fontWeight: FontWeight.w600)),
                      ]),
                      const SizedBox(height: 12),
                      if (p.status == PaymentStatus.pending && a.status == AppointmentStatus.pending) ..._payForm(l, inDrc),
                    ],
                  ]),
                ),
    );
  }

  Widget _line(IconData icon, String text) => Padding(
        padding: const EdgeInsets.only(top: 6),
        child: Row(crossAxisAlignment: CrossAxisAlignment.start, children: [
          Icon(icon, size: 18, color: Colors.grey[700]),
          const SizedBox(width: 8),
          Expanded(child: Text(text)),
        ]),
      );

  List<Widget> _payForm(L10n l, bool inDrc) {
    if (_waiting) {
      return [
        InfoBanner(l.t('checkPhone'), icon: Icons.phone_android),
        const SizedBox(height: 12),
        const Center(child: CircularProgressIndicator()),
      ];
    }
    return [
      Text(l.t('mobileMoney'), style: const TextStyle(fontWeight: FontWeight.w600)),
      const SizedBox(height: 8),
      TextField(controller: _name, decoration: InputDecoration(labelText: l.t('fullName'))),
      const SizedBox(height: 10),
      TextField(controller: _phone, keyboardType: TextInputType.phone, decoration: InputDecoration(labelText: l.t('mobileNumber'), hintText: '081 000 0000')),
      const SizedBox(height: 10),
      DropdownButtonFormField<_Operator>(
        initialValue: _operator,
        decoration: InputDecoration(labelText: l.t('operator')),
        items: const [
          DropdownMenuItem(value: _Operator.mpesa, child: Text('M-Pesa (Vodacom)')),
          DropdownMenuItem(value: _Operator.orange, child: Text('Orange Money')),
          DropdownMenuItem(value: _Operator.airtel, child: Text('Airtel Money')),
          DropdownMenuItem(value: _Operator.africell, child: Text('Africell (Afrimoney)')),
        ],
        onChanged: (v) => setState(() => _operator = v ?? _Operator.mpesa),
      ),
      if (_operator == _Operator.airtel) ...[const SizedBox(height: 8), Text(l.t('airtelFee'), style: TextStyle(color: Colors.grey[700], fontSize: 13))],
      if (_payError != null) ...[const SizedBox(height: 12), ErrorBanner(_payError!)],
      const SizedBox(height: 14),
      FilledButton(onPressed: _paying ? null : _payMobileMoney, child: _paying ? const ButtonSpinner() : Text(l.t('payNow'))),
      if (!inDrc) ...[
        const SizedBox(height: 10),
        OutlinedButton.icon(onPressed: _paying ? null : _payByCard, icon: const Icon(Icons.credit_card), label: Text(l.t('card'))),
      ],
      const SizedBox(height: 12),
      Text(l.t('insuranceOnWeb'), style: TextStyle(color: Colors.grey[600], fontSize: 13)),
    ];
  }
}
