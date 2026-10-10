import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:landadoc_common/landadoc_common.dart';

import 'appointment.dart';

// A doctor's profile and the next 14 days of free times, fetched in ONE request (the web page
// asks day by day). Picking a time, who it's for and a reason books it; the patient then lands
// on the appointment to pay.
class DoctorScreen extends StatefulWidget {
  final Doctor doctor;
  const DoctorScreen(this.doctor, {super.key});

  @override
  State<DoctorScreen> createState() => _DoctorScreenState();
}

class _DoctorScreenState extends State<DoctorScreen> {
  static const _days = 14;
  SlotRange? _range;
  String? _loadError;
  int _day = 0;
  String? _slot;
  List<FamilyPerson> _family = [];
  String? _forPatientId;   // null: for the signed-in patient
  final _motif = TextEditingController();
  bool _booking = false;
  String? _bookError;

  @override
  void initState() {
    super.initState();
    _load();
  }

  @override
  void dispose() {
    _motif.dispose();
    super.dispose();
  }

  Future<void> _load() async {
    final api = context.read<Session>().api;
    final l = context.read<L10n>();
    setState(() => _loadError = null);
    try {
      final now = DateTime.now();
      final results = await Future.wait([
        api.slots(widget.doctor.id, DateTime(now.year, now.month, now.day), _days),
        api.family().catchError((_) => <FamilyPerson>[]),
      ]);
      if (!mounted) return;
      setState(() {
        _range = results[0] as SlotRange;
        _family = results[1] as List<FamilyPerson>;
        // Open on the first day that still has a free time
        final first = _range!.days.indexWhere((d) => d.slots.isNotEmpty);
        _day = first < 0 ? 0 : first;
        _slot = null;
      });
    } on ApiException catch (e) {
      if (mounted) setState(() => _loadError = e.offline ? l.t('networkError') : (e.message ?? l.t('networkError')));
    }
  }

  Future<void> _book() async {
    final range = _range;
    final slot = _slot;
    if (range == null || slot == null || _booking) return;
    final l = context.read<L10n>();
    setState(() {
      _booking = true;
      _bookError = null;
    });
    try {
      final day = range.days[_day].date;
      final appointment = await context.read<Session>().api.book(
            widget.doctor.id, atSlot(day, slot), _motif.text.trim().isEmpty ? null : _motif.text.trim(), _forPatientId);
      if (!mounted) return;
      await Navigator.of(context).pushReplacement(MaterialPageRoute(builder: (_) => AppointmentScreen(appointment.id)));
    } on ApiException catch (e) {
      if (!mounted) return;
      setState(() => _bookError = e.offline ? l.t('networkError') : e.status == 409 ? l.t('slotTaken') : (e.message ?? l.t('bookFailed')));
      if (e.status == 409) await _load();   // the taken (or past) time disappears
    } finally {
      if (mounted) setState(() => _booking = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final d = widget.doctor;
    final range = _range;
    final dayFormat = DateFormat('EEE d', l.language);
    final today = DateTime.now();
    return Scaffold(
      appBar: AppBar(title: Text('${l.t('dr')} ${d.lastName}')),
      body: ListView(padding: const EdgeInsets.all(16), children: [
        Row(children: [
          DoctorAvatar(d, size: 72),
          const SizedBox(width: 14),
          Expanded(
            child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
              Text('${l.t('dr')} ${d.firstName} ${d.lastName}', style: Theme.of(context).textTheme.titleLarge),
              Text(d.specialty, style: TextStyle(color: Theme.of(context).colorScheme.primary)),
              Text('${l.t('fee')}: ${money(d.fee)}'),
            ]),
          ),
        ]),
        for (final c in d.clinics)
          Padding(
            padding: const EdgeInsets.only(top: 8),
            child: Row(children: [
              const Icon(Icons.location_on_outlined, size: 18),
              const SizedBox(width: 6),
              Expanded(child: Text([c.name, c.address, c.city].whereType<String>().where((s) => s.isNotEmpty).join(' · '))),
            ]),
          ),
        if ((d.bio ?? '').isNotEmpty) ...[
          const SizedBox(height: 16),
          Text(l.t('about'), style: Theme.of(context).textTheme.titleMedium),
          const SizedBox(height: 4),
          Text(d.bio!),
        ],
        const SizedBox(height: 20),
        Text(l.t('availableTimes'), style: Theme.of(context).textTheme.titleMedium),
        const SizedBox(height: 8),
        if (_loadError != null)
          RetryPanel(_loadError!, _load)
        else if (range == null)
          const Padding(padding: EdgeInsets.all(24), child: Center(child: CircularProgressIndicator()))
        else ...[
          SizedBox(
            height: 64,
            child: ListView.separated(
              scrollDirection: Axis.horizontal,
              itemCount: range.days.length,
              separatorBuilder: (_, _) => const SizedBox(width: 8),
              itemBuilder: (_, i) {
                final day = range.days[i];
                final isToday = DateUtils.isSameDay(day.date, today);
                final open = day.slots.length;
                return ChoiceChip(
                  selected: i == _day,
                  onSelected: (_) => setState(() {
                    _day = i;
                    _slot = null;
                  }),
                  label: Column(mainAxisSize: MainAxisSize.min, children: [
                    Text(isToday ? l.t('today') : dayFormat.format(day.date), style: const TextStyle(fontWeight: FontWeight.w600)),
                    Text('$open', style: TextStyle(fontSize: 12, color: open == 0 ? Colors.grey : null)),
                  ]),
                );
              },
            ),
          ),
          const SizedBox(height: 12),
          _SlotGrid(
            day: range.days[_day],
            zone: range.timeZone,
            selected: _slot,
            onSelected: (s) => setState(() => _slot = s),
          ),
          if (_slot != null) ...[
            const SizedBox(height: 20),
            if (_family.isNotEmpty)
              Padding(
                padding: const EdgeInsets.only(bottom: 12),
                child: DropdownButtonFormField<String?>(
                  initialValue: _forPatientId,
                  decoration: InputDecoration(labelText: l.t('bookFor')),
                  items: [
                    DropdownMenuItem(value: null, child: Text(l.t('myself'))),
                    for (final p in _family) DropdownMenuItem(value: p.id, child: Text('${p.name} (${p.relation})')),
                  ],
                  onChanged: (v) => setState(() => _forPatientId = v),
                ),
              ),
            TextField(controller: _motif, maxLength: 200, decoration: InputDecoration(labelText: l.t('reason'))),
            if (_bookError != null) ...[ErrorBanner(_bookError!), const SizedBox(height: 12)],
            FilledButton(
              onPressed: _booking ? null : _book,
              child: _booking
                  ? const ButtonSpinner()
                  : Text('${l.t('book')} · ${dayFormat.format(range.days[_day].date)} ${_slot!}'),
            ),
          ],
        ],
      ]),
    );
  }
}

class _SlotGrid extends StatelessWidget {
  final DaySlots day;
  final String? zone;
  final String? selected;
  final ValueChanged<String> onSelected;
  const _SlotGrid({required this.day, required this.zone, required this.selected, required this.onSelected});

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    if (day.slots.isEmpty) return Text(l.t('noSlotsDay'), style: TextStyle(color: Colors.grey[600]));
    final phoneTimes = {for (final s in day.slots) s: onPhoneClock(atSlot(day.date, s), zone)};
    final differs = phoneTimes.values.any((t) => t != null);
    return Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
      if (differs) Padding(padding: const EdgeInsets.only(bottom: 8), child: InfoBanner(l.f('zoneNote', [cityOf(zone)]), icon: Icons.schedule)),
      Wrap(spacing: 8, runSpacing: 8, children: [
        for (final s in day.slots)
          ChoiceChip(
            selected: s == selected,
            onSelected: (_) => onSelected(s),
            label: Column(mainAxisSize: MainAxisSize.min, children: [
              Text(s, style: const TextStyle(fontWeight: FontWeight.w600)),
              if (phoneTimes[s] != null) Text(l.f('yourTime', [hhmm(phoneTimes[s]!)]), style: const TextStyle(fontSize: 11)),
            ]),
          ),
      ]),
    ]);
  }
}
