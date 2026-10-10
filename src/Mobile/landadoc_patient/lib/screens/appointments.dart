import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:shared_preferences/shared_preferences.dart';

import '../api.dart';
import '../l10n.dart';
import '../models.dart';
import '../session.dart';
import '../widgets.dart';
import 'appointment.dart';

// The patient's appointments, upcoming first. The last list is saved on the phone, so it still
// opens (read-only) without a connection.
class AppointmentsScreen extends StatefulWidget {
  const AppointmentsScreen({super.key});

  @override
  State<AppointmentsScreen> createState() => AppointmentsScreenState();
}

class AppointmentsScreenState extends State<AppointmentsScreen> {
  List<Appointment>? _items;
  bool _offline = false;
  bool _loading = false;
  String? _error;

  @override
  void initState() {
    super.initState();
    reload();
  }

  Future<void> reload() async {
    if (_loading) return;
    final session = context.read<Session>();
    final l = context.read<L10n>();
    setState(() {
      _loading = true;
      _error = null;
    });
    final prefs = await SharedPreferences.getInstance();
    try {
      final json = await session.api.myAppointmentsRaw();
      await prefs.setString(Session.appointmentsCacheKey, json);
      if (mounted) {
        setState(() {
          _items = Api.parseAppointments(json);
          _offline = false;
        });
      }
    } on ApiException catch (e) {
      final saved = prefs.getString(Session.appointmentsCacheKey);
      if (!mounted) return;
      if (e.offline && saved != null) {
        setState(() {
          _items = Api.parseAppointments(saved);
          _offline = true;
        });
      } else {
        setState(() => _error = e.offline ? l.t('networkError') : (e.message ?? l.t('networkError')));
      }
    } finally {
      if (mounted) setState(() => _loading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final items = _items;
    final now = DateTime.now();
    final upcoming = (items ?? []).where((a) => !a.slotStart.isBefore(DateTime(now.year, now.month, now.day))).toList()
      ..sort((a, b) => a.slotStart.compareTo(b.slotStart));
    final past = (items ?? []).where((a) => a.slotStart.isBefore(DateTime(now.year, now.month, now.day))).toList()
      ..sort((a, b) => b.slotStart.compareTo(a.slotStart));

    return Scaffold(
      appBar: AppBar(title: Text(l.t('myAppointments'))),
      body: _error != null && items == null
          ? RetryPanel(_error!, reload)
          : items == null
              ? const Center(child: CircularProgressIndicator())
              : RefreshIndicator(
                  onRefresh: reload,
                  child: ListView(padding: const EdgeInsets.all(16), children: [
                    if (_offline) ...[InfoBanner(l.t('offlineCopy'), icon: Icons.cloud_off), const SizedBox(height: 12)],
                    if (items.isEmpty) Padding(padding: const EdgeInsets.only(top: 48), child: Center(child: Text(l.t('noAppointments')))),
                    if (upcoming.isNotEmpty) _Section(l.t('upcoming'), upcoming, onChanged: reload),
                    if (past.isNotEmpty) _Section(l.t('past'), past, onChanged: reload),
                  ]),
                ),
    );
  }
}

class _Section extends StatelessWidget {
  final String title;
  final List<Appointment> items;
  final VoidCallback onChanged;
  const _Section(this.title, this.items, {required this.onChanged});

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final format = DateFormat('EEE d MMM · HH:mm', l.language);
    return Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
      Padding(padding: const EdgeInsets.fromLTRB(4, 8, 4, 8), child: Text(title, style: Theme.of(context).textTheme.titleMedium)),
      for (final a in items)
        Card(
          elevation: 0,
          shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12), side: BorderSide(color: Colors.grey.shade200)),
          child: ListTile(
            title: Text(a.doctorName ?? '—', style: const TextStyle(fontWeight: FontWeight.w600)),
            subtitle: Text([format.format(a.slotStart), if (a.clinicName != null) a.clinicName!].join('\n')),
            isThreeLine: a.clinicName != null,
            trailing: StatusChip(a.status, l.t(a.statusKey)),
            onTap: () async {
              await Navigator.of(context).push(MaterialPageRoute(builder: (_) => AppointmentScreen(a.id)));
              onChanged();
            },
          ),
        ),
    ]);
  }
}

class StatusChip extends StatelessWidget {
  final AppointmentStatus status;
  final String label;
  const StatusChip(this.status, this.label, {super.key});

  @override
  Widget build(BuildContext context) {
    final (bg, fg) = switch (status) {
      AppointmentStatus.confirmed => (const Color(0xFFDCFCE7), const Color(0xFF166534)),
      AppointmentStatus.completed => (const Color(0xFFE0E7FF), const Color(0xFF3730A3)),
      AppointmentStatus.cancelled => (const Color(0xFFFEE2E2), const Color(0xFF991B1B)),
      _ => (const Color(0xFFFEF3C7), const Color(0xFF92400E)),
    };
    return Container(
      padding: const EdgeInsets.symmetric(horizontal: 8, vertical: 4),
      decoration: BoxDecoration(color: bg, borderRadius: BorderRadius.circular(20)),
      child: Text(label, style: TextStyle(color: fg, fontSize: 12, fontWeight: FontWeight.w600)),
    );
  }
}
