import 'dart:async';

import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:landadoc_common/landadoc_common.dart';

import 'doctor.dart';

// Doctors matching a name, specialty or city. The search waits until the patient stops typing
// (no request per letter), and photos come separately and are cached.
class SearchScreen extends StatefulWidget {
  final VoidCallback onBooked;
  const SearchScreen({required this.onBooked, super.key});

  @override
  State<SearchScreen> createState() => _SearchScreenState();
}

class _SearchScreenState extends State<SearchScreen> {
  final _query = TextEditingController();
  Timer? _debounce;
  List<Doctor>? _doctors;
  String? _error;
  bool _loading = false;

  @override
  void initState() {
    super.initState();
    _load();
  }

  @override
  void dispose() {
    _debounce?.cancel();
    _query.dispose();
    super.dispose();
  }

  void _onChanged(String _) {
    _debounce?.cancel();
    _debounce = Timer(const Duration(milliseconds: 450), _load);
  }

  Future<void> _load() async {
    final l = context.read<L10n>();
    setState(() {
      _loading = true;
      _error = null;
    });
    try {
      final doctors = await context.read<Session>().api.searchDoctors(_query.text.trim());
      if (mounted) setState(() => _doctors = doctors);
    } on ApiException catch (e) {
      if (mounted) setState(() => _error = e.offline ? l.t('networkError') : (e.message ?? l.t('networkError')));
    } finally {
      if (mounted) setState(() => _loading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final doctors = _doctors;
    return Scaffold(
      appBar: AppBar(title: Text(l.t('findDoctor'))),
      body: Column(children: [
        Padding(
          padding: const EdgeInsets.fromLTRB(16, 12, 16, 8),
          child: TextField(
            controller: _query,
            onChanged: _onChanged,
            textInputAction: TextInputAction.search,
            onSubmitted: (_) => _load(),
            decoration: InputDecoration(prefixIcon: const Icon(Icons.search), hintText: l.t('searchHint')),
          ),
        ),
        if (_loading) const LinearProgressIndicator(minHeight: 2),
        Expanded(
          child: _error != null && doctors == null
              ? RetryPanel(_error!, _load)
              : doctors == null
                  ? const SizedBox.shrink()
                  : doctors.isEmpty
                      ? Center(child: Text(l.t('noDoctors')))
                      : RefreshIndicator(
                          onRefresh: _load,
                          child: ListView.separated(
                            padding: const EdgeInsets.fromLTRB(16, 8, 16, 24),
                            itemCount: doctors.length,
                            separatorBuilder: (_, _) => const SizedBox(height: 10),
                            itemBuilder: (_, i) => _DoctorCard(doctors[i], onBooked: widget.onBooked),
                          ),
                        ),
        ),
      ]),
    );
  }
}

class _DoctorCard extends StatelessWidget {
  final Doctor doctor;
  final VoidCallback onBooked;
  const _DoctorCard(this.doctor, {required this.onBooked});

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final clinic = doctor.clinics.isEmpty ? null : doctor.clinics.first;
    return Card(
      elevation: 0,
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(12), side: BorderSide(color: Colors.grey.shade200)),
      child: InkWell(
        borderRadius: BorderRadius.circular(12),
        onTap: () async {
          final booked = await Navigator.of(context).push<bool>(MaterialPageRoute(builder: (_) => DoctorScreen(doctor)));
          if (booked == true) onBooked();
        },
        child: Padding(
          padding: const EdgeInsets.all(12),
          child: Row(children: [
            DoctorAvatar(doctor, size: 52),
            const SizedBox(width: 12),
            Expanded(
              child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
                Text('${l.t('dr')} ${doctor.firstName} ${doctor.lastName}', style: const TextStyle(fontWeight: FontWeight.w600, fontSize: 15)),
                Text(doctor.specialty, style: TextStyle(color: Theme.of(context).colorScheme.primary)),
                if (clinic != null) Text('${clinic.name} · ${clinic.city}', style: TextStyle(color: Colors.grey[600], fontSize: 13), maxLines: 1, overflow: TextOverflow.ellipsis),
              ]),
            ),
            Column(crossAxisAlignment: CrossAxisAlignment.end, children: [
              Text(money(doctor.fee), style: const TextStyle(fontWeight: FontWeight.w700)),
              if (doctor.ratingCount > 0)
                Row(mainAxisSize: MainAxisSize.min, children: [
                  const Icon(Icons.star_rounded, size: 16, color: Color(0xFFF59E0B)),
                  Text(doctor.rating.toStringAsFixed(1), style: const TextStyle(fontSize: 13)),
                ]),
            ]),
          ]),
        ),
      ),
    );
  }
}
