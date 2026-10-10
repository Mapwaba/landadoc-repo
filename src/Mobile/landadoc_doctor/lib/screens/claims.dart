import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:landadoc_common/landadoc_common.dart';
import 'package:provider/provider.dart';

// Insurance claims: the ones waiting for the doctor (approve with the insurer's reference, or
// decline), then the approved ones the insurer still owes. Settling and the history are on the website.
class ClaimsScreen extends StatefulWidget {
  const ClaimsScreen({super.key});

  @override
  State<ClaimsScreen> createState() => ClaimsScreenState();
}

class ClaimsScreenState extends State<ClaimsScreen> {
  List<InsuranceClaim>? _claims;
  Map<String, String> _patientNames = {};
  String? _error;
  bool _loading = false;

  @override
  void initState() {
    super.initState();
    reload();
  }

  Future<void> reload() async {
    if (_loading) return;
    final api = context.read<Session>().api;
    final l = context.read<L10n>();
    setState(() {
      _loading = true;
      _error = null;
    });
    try {
      final results = await Future.wait([
        api.myClaims(),
        api.myPatients().catchError((_) => <PatientContact>[]),
      ]);
      if (!mounted) return;
      setState(() {
        _claims = results[0] as List<InsuranceClaim>;
        _patientNames = {for (final p in results[1] as List<PatientContact>) p.id: p.fullName};
      });
    } on ApiException catch (e) {
      if (mounted) setState(() => _error = e.offline ? l.t('networkError') : (e.message ?? l.t('networkError')));
    } finally {
      if (mounted) setState(() => _loading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final claims = _claims;
    final toReview = (claims ?? []).where((c) => c.status == ClaimStatus.submitted).toList();
    final owed = (claims ?? []).where((c) => c.status == ClaimStatus.approved).toList();
    return Scaffold(
      appBar: AppBar(title: Text(l.t('claims'))),
      body: _error != null && claims == null
          ? RetryPanel(_error!, reload)
          : claims == null
              ? const Center(child: CircularProgressIndicator())
              : RefreshIndicator(
                  onRefresh: reload,
                  child: ListView(padding: const EdgeInsets.all(16), children: [
                    Text(l.t('claimsToReview'), style: Theme.of(context).textTheme.titleMedium),
                    const SizedBox(height: 8),
                    if (toReview.isEmpty) Text(l.t('noClaims'), style: TextStyle(color: Colors.grey[600])),
                    for (final c in toReview) _ClaimCard(c, _patientNames[c.patientId], onChanged: reload),
                    if (owed.isNotEmpty) ...[
                      const SizedBox(height: 24),
                      Text(l.t('claimsOwed'), style: Theme.of(context).textTheme.titleMedium),
                      const SizedBox(height: 8),
                      for (final c in owed) _ClaimCard(c, _patientNames[c.patientId], onChanged: reload),
                    ],
                  ]),
                ),
    );
  }
}

class _ClaimCard extends StatelessWidget {
  final InsuranceClaim claim;
  final String? patientName;
  final Future<void> Function() onChanged;
  const _ClaimCard(this.claim, this.patientName, {required this.onChanged});

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final c = claim;
    final review = c.status == ClaimStatus.submitted;
    return Card(
      elevation: 0,
      margin: const EdgeInsets.only(bottom: 10),
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12), side: BorderSide(color: Colors.grey.shade200)),
      child: Padding(
        padding: const EdgeInsets.all(14),
        child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
          Row(children: [
            Expanded(child: Text(patientName ?? l.t('patient'), style: const TextStyle(fontWeight: FontWeight.w600, fontSize: 15))),
            Text(money(c.amount), style: const TextStyle(fontWeight: FontWeight.w700)),
          ]),
          const SizedBox(height: 4),
          Text(c.insurerName, style: TextStyle(color: Theme.of(context).colorScheme.primary)),
          Text(l.f('member', [c.memberNumber])),
          if ((c.memberName ?? '').isNotEmpty) Text(l.f('mainMember', [c.memberName!])),
          if (c.createdAt != null) Text(DateFormat('d MMM y', l.language).format(c.createdAt!.toLocal()), style: TextStyle(color: Colors.grey[600], fontSize: 13)),
          if ((c.authorizationReference ?? '').isNotEmpty) Text('Ref. ${c.authorizationReference}', style: TextStyle(color: Colors.grey[700], fontSize: 13)),
          if (review) ...[
            const SizedBox(height: 12),
            Row(children: [
              Expanded(child: OutlinedButton(onPressed: () => _decline(context, l), child: Text(l.t('decline')))),
              const SizedBox(width: 10),
              Expanded(child: FilledButton(onPressed: () => _approve(context, l), child: Text(l.t('approve')))),
            ]),
          ],
        ]),
      ),
    );
  }

  Future<void> _approve(BuildContext context, L10n l) async {
    final reference = await _ask(context, l, title: l.t('approve'), hint: l.t('approveHint'), label: l.t('insurerReference'), required: true);
    if (reference == null || !context.mounted) return;
    await _run(context, l, () => context.read<Session>().api.approveClaim(claim.id, reference));
  }

  Future<void> _decline(BuildContext context, L10n l) async {
    final reason = await _ask(context, l, title: l.t('decline'), label: l.t('declineReason'), required: false);
    if (reason == null || !context.mounted) return;
    await _run(context, l, () => context.read<Session>().api.declineClaim(claim.id, reason.isEmpty ? null : reason));
  }

  Future<void> _run(BuildContext context, L10n l, Future<void> Function() call) async {
    final messenger = ScaffoldMessenger.of(context);
    try {
      await call();
      await onChanged();
    } on ApiException catch (e) {
      messenger.showSnackBar(SnackBar(
          content: Text(e.offline ? l.t('networkError') : e.status == 409 ? l.t('claimDone') : (e.message ?? l.t('actionFailed')))));
      await onChanged();
    }
  }

  // A short text in a dialog; null when cancelled. A required answer can't be empty.
  static Future<String?> _ask(BuildContext context, L10n l, {required String title, String? hint, required String label, required bool required}) {
    final controller = TextEditingController();
    return showDialog<String>(
      context: context,
      builder: (d) => StatefulBuilder(
        builder: (d, setState) => AlertDialog(
          title: Text(title),
          content: Column(mainAxisSize: MainAxisSize.min, crossAxisAlignment: CrossAxisAlignment.start, children: [
            if (hint != null) Padding(padding: const EdgeInsets.only(bottom: 12), child: Text(hint)),
            TextField(controller: controller, autofocus: true, decoration: InputDecoration(labelText: label), onChanged: (_) => setState(() {})),
          ]),
          actions: [
            TextButton(onPressed: () => Navigator.of(d).pop(), child: Text(l.t('cancel'))),
            FilledButton(
              onPressed: required && controller.text.trim().isEmpty ? null : () => Navigator.of(d).pop(controller.text.trim()),
              child: Text(l.t('confirm')),
            ),
          ],
        ),
      ),
    );
  }
}
