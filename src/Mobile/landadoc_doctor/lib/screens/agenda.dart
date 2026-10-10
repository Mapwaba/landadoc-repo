import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:landadoc_common/landadoc_common.dart';
import 'package:provider/provider.dart';
import 'package:shared_preferences/shared_preferences.dart';

import 'patients.dart';

// The doctor's day: appointments (readable offline), and the day's time slots — open, booked or
// blocked — where a tap blocks an open slot or reopens a blocked one. Days run from 3 days ago
// (to complete recent visits) to 2 weeks ahead.
class AgendaScreen extends StatefulWidget {
  const AgendaScreen({super.key});

  @override
  State<AgendaScreen> createState() => _AgendaScreenState();
}

class _AgendaScreenState extends State<AgendaScreen> {
  static const _pastDays = 3, _aheadDays = 14;
  late final DateTime _today;
  late DateTime _selected;

  List<Appointment>? _appointments;
  bool _offline = false;
  String? _error;
  Map<int, ScheduleDay> _schedule = {};
  Map<DateTime, Set<String>> _open = {};       // day → open "HH:mm"
  Set<DateTime> _blocked = {};                  // blocked slot starts
  bool _slotsLoading = false;
  bool _changing = false;

  @override
  void initState() {
    super.initState();
    final now = DateTime.now();
    _today = DateTime(now.year, now.month, now.day);
    _selected = _today;
    _load();
  }

  List<DateTime> get _days => [for (var i = -_pastDays; i < _aheadDays; i++) _today.add(Duration(days: i))];

  Future<void> _load() async {
    await Future.wait([_loadAppointments(), _loadSlots()]);
  }

  Future<void> _loadAppointments() async {
    final session = context.read<Session>();
    final l = context.read<L10n>();
    final prefs = await SharedPreferences.getInstance();
    try {
      final json = await session.api.doctorAgendaRaw();
      await prefs.setString(session.appointmentsCacheKey, json);
      if (mounted) {
        setState(() {
          _appointments = Api.parseAppointments(json);
          _offline = false;
          _error = null;
        });
      }
    } on ApiException catch (e) {
      final saved = prefs.getString(session.appointmentsCacheKey);
      if (!mounted) return;
      setState(() {
        if (e.offline && saved != null) {
          _appointments = Api.parseAppointments(saved);
          _offline = true;
        } else {
          _error = e.offline ? l.t('networkError') : (e.message ?? l.t('networkError'));
        }
      });
    }
  }

  // The weekly template, open slots and blocked slots from today on (the past can't change)
  Future<void> _loadSlots() async {
    final session = context.read<Session>();
    final doctorId = session.userId;
    if (doctorId == null) return;
    setState(() => _slotsLoading = true);
    try {
      final results = await Future.wait([
        session.api.mySchedule(),
        session.api.slots(doctorId, _today, _aheadDays),
        session.api.myBlocked(_today, _aheadDays),
      ]);
      if (!mounted) return;
      setState(() {
        _schedule = {for (final d in results[0] as List<ScheduleDay>) d.weekday: d};
        _open = {for (final d in (results[1] as SlotRange).days) DateUtils.dateOnly(d.date): d.slots.toSet()};
        _blocked = (results[2] as List<DateTime>).toSet();
      });
    } catch (_) {
      // the appointments still show; the slot section offers a retry
      if (mounted) setState(() => _open = {});
    } finally {
      if (mounted) setState(() => _slotsLoading = false);
    }
  }

  Future<void> _setBlocked(List<DateTime> slots, bool block) async {
    if (slots.isEmpty || _changing) return;
    final api = context.read<Session>().api;
    final l = context.read<L10n>();
    final messenger = ScaffoldMessenger.of(context);
    setState(() => _changing = true);
    try {
      if (block) {
        await api.block(slots);
      } else {
        await api.unblock(slots);
      }
      await _loadSlots();
    } on ApiException catch (e) {
      messenger.showSnackBar(SnackBar(content: Text(e.offline ? l.t('networkError') : (e.message ?? l.t('actionFailed')))));
    } finally {
      if (mounted) setState(() => _changing = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final dayFormat = DateFormat('EEE d', l.language);
    final titleFormat = DateFormat('EEEE d MMMM', l.language);
    final appointments = (_appointments ?? []).where((a) => DateUtils.isSameDay(a.slotStart, _selected)).toList()
      ..sort((a, b) => a.slotStart.compareTo(b.slotStart));

    return Scaffold(
      appBar: AppBar(title: Text(l.t('agenda'))),
      body: _error != null && _appointments == null
          ? RetryPanel(_error!, _load)
          : RefreshIndicator(
              onRefresh: _load,
              child: ListView(padding: const EdgeInsets.only(bottom: 24), children: [
                SizedBox(
                  height: 64,
                  child: ListView.separated(
                    scrollDirection: Axis.horizontal,
                    padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
                    itemCount: _days.length,
                    controller: ScrollController(initialScrollOffset: _pastDays * 76.0),
                    separatorBuilder: (_, _) => const SizedBox(width: 8),
                    itemBuilder: (_, i) {
                      final day = _days[i];
                      final count = (_appointments ?? [])
                          .where((a) => DateUtils.isSameDay(a.slotStart, day) && a.status != AppointmentStatus.cancelled)
                          .length;
                      return ChoiceChip(
                        selected: DateUtils.isSameDay(day, _selected),
                        onSelected: (_) => setState(() => _selected = day),
                        label: Column(mainAxisSize: MainAxisSize.min, children: [
                          Text(DateUtils.isSameDay(day, _today) ? l.t('today') : dayFormat.format(day), style: const TextStyle(fontWeight: FontWeight.w600)),
                          Text('$count', style: TextStyle(fontSize: 12, color: count == 0 ? Colors.grey : null)),
                        ]),
                      );
                    },
                  ),
                ),
                if (_offline) Padding(padding: const EdgeInsets.fromLTRB(16, 4, 16, 4), child: InfoBanner(l.t('offlineCopy'), icon: Icons.cloud_off)),
                Padding(
                  padding: const EdgeInsets.fromLTRB(16, 8, 16, 8),
                  child: Text(titleFormat.format(_selected), style: Theme.of(context).textTheme.titleMedium),
                ),
                if (_appointments == null)
                  const Padding(padding: EdgeInsets.all(24), child: Center(child: CircularProgressIndicator()))
                else if (appointments.isEmpty)
                  Padding(padding: const EdgeInsets.symmetric(horizontal: 16), child: Text(l.t('noAppointmentsDay'), style: TextStyle(color: Colors.grey[600])))
                else
                  for (final a in appointments) _AppointmentTile(a, onChanged: _loadAppointments),
                if (!_selected.isBefore(_today)) _slotSection(l),
              ]),
            ),
    );
  }

  Widget _slotSection(L10n l) {
    final schedule = _schedule[_selected.weekday];
    final open = _open[_selected];
    final booked = {
      for (final a in _appointments ?? <Appointment>[])
        if (DateUtils.isSameDay(a.slotStart, _selected) && a.status != AppointmentStatus.cancelled) hhmm(a.slotStart),
    };
    final blocked = {for (final b in _blocked) if (DateUtils.isSameDay(b, _selected)) hhmm(b)};

    final children = <Widget>[
      const Divider(height: 32),
      Row(children: [
        Text(l.t('slots'), style: Theme.of(context).textTheme.titleMedium),
        const Spacer(),
        if (_slotsLoading || _changing) const SizedBox(width: 18, height: 18, child: CircularProgressIndicator(strokeWidth: 2)),
      ]),
      const SizedBox(height: 8),
    ];

    if (schedule == null) {
      children.add(Text(l.t('notWorkingDay'), style: TextStyle(color: Colors.grey[600])));
    } else if (open == null) {
      children.add(Align(alignment: Alignment.centerLeft, child: OutlinedButton(onPressed: _loadSlots, child: Text(l.t('retry')))));
    } else {
      // Slots that have already started today are neither open nor blocked any more: hidden
      final slots = schedule.slots().where((s) => booked.contains(s) || blocked.contains(s) || open.contains(s)).toList();
      final openSlots = slots.where((s) => open.contains(s) && !booked.contains(s)).toList();
      final blockedSlots = slots.where((s) => blocked.contains(s)).toList();
      children.addAll([
        Text(l.t('slotsHint'), style: TextStyle(color: Colors.grey[700], fontSize: 13)),
        const SizedBox(height: 10),
        Wrap(spacing: 8, runSpacing: 8, children: [
          for (final s in slots) _slotChip(l, s, booked.contains(s) ? 'booked' : blocked.contains(s) ? 'blocked' : 'open'),
        ]),
        const SizedBox(height: 14),
        Wrap(spacing: 8, runSpacing: 8, children: [
          if (openSlots.isNotEmpty)
            OutlinedButton.icon(
              onPressed: _changing ? null : () => _setBlocked([for (final s in openSlots) atSlot(_selected, s)], true),
              icon: const Icon(Icons.block, size: 18),
              label: Text(l.t('blockDay')),
            ),
          if (blockedSlots.isNotEmpty)
            OutlinedButton.icon(
              onPressed: _changing ? null : () => _setBlocked([for (final s in blockedSlots) atSlot(_selected, s)], false),
              icon: const Icon(Icons.lock_open, size: 18),
              label: Text(l.t('unblockDay')),
            ),
        ]),
      ]);
    }
    return Padding(padding: const EdgeInsets.symmetric(horizontal: 16), child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: children));
  }

  Widget _slotChip(L10n l, String slot, String state) {
    final (bg, fg, label) = switch (state) {
      'booked' => (const Color(0xFFDBEAFE), const Color(0xFF1E40AF), l.t('slotBooked')),
      'blocked' => (const Color(0xFFF1F5F9), const Color(0xFF64748B), l.t('slotBlocked')),
      _ => (const Color(0xFFDCFCE7), const Color(0xFF166534), l.t('slotOpen')),
    };
    return Material(
      color: bg,
      borderRadius: BorderRadius.circular(8),
      child: InkWell(
        borderRadius: BorderRadius.circular(8),
        onTap: state == 'booked' || _changing ? null : () => _setBlocked([atSlot(_selected, slot)], state == 'open'),
        child: Padding(
          padding: const EdgeInsets.symmetric(horizontal: 12, vertical: 8),
          child: Column(mainAxisSize: MainAxisSize.min, children: [
            Text(slot, style: TextStyle(fontWeight: FontWeight.w700, color: fg, decoration: state == 'blocked' ? TextDecoration.lineThrough : null)),
            Text(label, style: TextStyle(fontSize: 11, color: fg)),
          ]),
        ),
      ),
    );
  }
}

class _AppointmentTile extends StatelessWidget {
  final Appointment appointment;
  final Future<void> Function() onChanged;
  const _AppointmentTile(this.appointment, {required this.onChanged});

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final a = appointment;
    return Card(
      margin: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
      elevation: 0,
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12), side: BorderSide(color: Colors.grey.shade200)),
      child: ListTile(
        leading: Text(hhmm(a.slotStart), style: const TextStyle(fontWeight: FontWeight.w700, fontSize: 16)),
        title: Text(a.patientName ?? l.t('patient'), style: const TextStyle(fontWeight: FontWeight.w600)),
        subtitle: (a.motif ?? '').isEmpty ? null : Text(a.motif!, maxLines: 2, overflow: TextOverflow.ellipsis),
        trailing: AppointmentStatusChip(a.status, l.t(a.statusKey)),
        onTap: () => _showActions(context, l),
      ),
    );
  }

  void _showActions(BuildContext context, L10n l) {
    final a = appointment;
    final canComplete = a.status == AppointmentStatus.pending || a.status == AppointmentStatus.confirmed;
    showModalBottomSheet<void>(
      context: context,
      showDragHandle: true,
      builder: (sheet) => SafeArea(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(20, 0, 20, 20),
          child: Column(mainAxisSize: MainAxisSize.min, crossAxisAlignment: CrossAxisAlignment.stretch, children: [
            Text(a.patientName ?? l.t('patient'), style: Theme.of(sheet).textTheme.titleLarge),
            Text('${DateFormat('EEEE d MMMM · HH:mm', l.language).format(a.slotStart)} · ${l.f('ref', [a.refNumber])}'),
            if ((a.motif ?? '').isNotEmpty) Padding(padding: const EdgeInsets.only(top: 8), child: Text(a.motif!)),
            const SizedBox(height: 16),
            OutlinedButton.icon(
              icon: const Icon(Icons.folder_shared_outlined),
              label: Text(l.t('openFile')),
              onPressed: () {
                Navigator.of(sheet).pop();
                Navigator.of(context).push(MaterialPageRoute(builder: (_) => PatientFileScreen(a.patientId, a.patientName)));
              },
            ),
            if (canComplete) ...[
              const SizedBox(height: 8),
              FilledButton.icon(
                icon: const Icon(Icons.check),
                label: Text(l.t('markCompleted')),
                onPressed: () async {
                  Navigator.of(sheet).pop();
                  await _complete(context, l);
                },
              ),
            ],
          ]),
        ),
      ),
    );
  }

  Future<void> _complete(BuildContext context, L10n l) async {
    final messenger = ScaffoldMessenger.of(context);
    final api = context.read<Session>().api;
    final ok = await showDialog<bool>(
      context: context,
      builder: (d) => AlertDialog(
        content: Text(l.t('completeQuestion')),
        actions: [
          TextButton(onPressed: () => Navigator.of(d).pop(false), child: Text(l.t('cancel'))),
          FilledButton(onPressed: () => Navigator.of(d).pop(true), child: Text(l.t('confirm'))),
        ],
      ),
    );
    if (ok != true) return;
    try {
      await api.completeAppointment(appointment.id);
      await onChanged();
    } on ApiException catch (e) {
      messenger.showSnackBar(SnackBar(content: Text(e.offline ? l.t('networkError') : (e.message ?? l.t('actionFailed')))));
    }
  }
}
