import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:landadoc_common/landadoc_common.dart';
import 'package:provider/provider.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:url_launcher/url_launcher.dart';

// The doctor's patients (those they've seen or who booked them), searchable by name
class PatientsScreen extends StatefulWidget {
  const PatientsScreen({super.key});

  @override
  State<PatientsScreen> createState() => _PatientsScreenState();
}

class _PatientsScreenState extends State<PatientsScreen> {
  List<PatientContact>? _patients;
  String? _error;
  String _query = '';

  @override
  void initState() {
    super.initState();
    _load();
  }

  Future<void> _load() async {
    final l = context.read<L10n>();
    try {
      final patients = await context.read<Session>().api.myPatients();
      if (mounted) setState(() => _patients = patients..sort((a, b) => a.lastName.toLowerCase().compareTo(b.lastName.toLowerCase())));
    } on ApiException catch (e) {
      if (mounted) setState(() => _error = e.offline ? l.t('networkError') : (e.message ?? l.t('networkError')));
    }
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final q = _query.trim().toLowerCase();
    final shown = (_patients ?? []).where((p) => q.isEmpty || p.fullName.toLowerCase().contains(q)).toList();
    return Scaffold(
      appBar: AppBar(title: Text(l.t('patients'))),
      body: _error != null && _patients == null
          ? RetryPanel(_error!, () {
              setState(() => _error = null);
              _load();
            })
          : _patients == null
              ? const Center(child: CircularProgressIndicator())
              : Column(children: [
                  Padding(
                    padding: const EdgeInsets.fromLTRB(16, 12, 16, 8),
                    child: TextField(
                      onChanged: (v) => setState(() => _query = v),
                      decoration: InputDecoration(prefixIcon: const Icon(Icons.search), hintText: l.t('searchPatient')),
                    ),
                  ),
                  Expanded(
                    child: shown.isEmpty
                        ? Center(child: Text(l.t('noPatients')))
                        : RefreshIndicator(
                            onRefresh: _load,
                            child: ListView.builder(
                              itemCount: shown.length,
                              itemBuilder: (_, i) {
                                final p = shown[i];
                                return ListTile(
                                  leading: PatientAvatar(p),
                                  title: Text(p.fullName),
                                  subtitle: Text(p.guardianName != null ? l.f('guardian', [p.guardianName!]) : (p.phone ?? p.email ?? '')),
                                  onTap: () => Navigator.of(context).push(MaterialPageRoute(builder: (_) => PatientFileScreen(p.id, p.fullName, contact: p))),
                                );
                              },
                            ),
                          ),
                  ),
                ]),
    );
  }
}

// The patient's photo (only their doctors see it) or their initials
class PatientAvatar extends StatelessWidget {
  final PatientContact patient;
  final double size;
  const PatientAvatar(this.patient, {this.size = 44, super.key});

  @override
  Widget build(BuildContext context) {
    final photo = patient.photoDataUrl;
    final comma = photo?.indexOf(',') ?? -1;
    if (photo != null && comma > 0) {
      try {
        return ClipOval(child: Image.memory(base64Decode(photo.substring(comma + 1)), width: size, height: size, fit: BoxFit.cover, gaplessPlayback: true));
      } catch (_) {}
    }
    final initials = '${patient.firstName.isNotEmpty ? patient.firstName[0] : ''}${patient.lastName.isNotEmpty ? patient.lastName[0] : ''}'.toUpperCase();
    return CircleAvatar(
      radius: size / 2,
      backgroundColor: brandSoft,
      child: Text(initials, style: TextStyle(color: Theme.of(context).colorScheme.primary, fontWeight: FontWeight.w700)),
    );
  }
}

// One patient: contact details, their appointments with this doctor, and their documents
class PatientFileScreen extends StatefulWidget {
  final String patientId;
  final String? name;
  final PatientContact? contact;
  const PatientFileScreen(this.patientId, this.name, {this.contact, super.key});

  @override
  State<PatientFileScreen> createState() => _PatientFileScreenState();
}

class _PatientFileScreenState extends State<PatientFileScreen> {
  PatientContact? _contact;
  List<MedicalDocument>? _documents;
  List<Appointment> _appointments = [];
  String? _error;

  @override
  void initState() {
    super.initState();
    _contact = widget.contact;
    _load();
  }

  Future<void> _load() async {
    final session = context.read<Session>();
    final l = context.read<L10n>();
    // Appointments from the agenda's saved copy (no extra request)
    final prefs = await SharedPreferences.getInstance();
    final saved = prefs.getString(session.appointmentsCacheKey);
    if (saved != null) {
      _appointments = Api.parseAppointments(saved).where((a) => a.patientId == widget.patientId).toList()
        ..sort((a, b) => b.slotStart.compareTo(a.slotStart));
    }
    try {
      final results = await Future.wait([
        session.api.documentsOf(widget.patientId),
        if (_contact == null) session.api.myPatients(),
      ]);
      if (!mounted) return;
      setState(() {
        _documents = results[0] as List<MedicalDocument>;
        if (results.length > 1) {
          final match = (results[1] as List<PatientContact>).where((p) => p.id == widget.patientId);
          if (match.isNotEmpty) _contact = match.first;
        }
      });
    } on ApiException catch (e) {
      if (mounted) setState(() => _error = e.offline ? l.t('networkError') : (e.message ?? l.t('networkError')));
    }
  }

  Future<void> _open(MedicalDocument doc) async {
    final l = context.read<L10n>();
    final messenger = ScaffoldMessenger.of(context);
    try {
      final url = await context.read<Session>().api.documentUrl(doc.id);
      if (url != null) await launchUrl(Uri.parse(url), mode: LaunchMode.externalApplication);
    } on ApiException catch (e) {
      messenger.showSnackBar(SnackBar(content: Text(e.offline ? l.t('networkError') : (e.message ?? l.t('actionFailed')))));
    }
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final c = _contact;
    final format = DateFormat('d MMM y · HH:mm', l.language);
    return Scaffold(
      appBar: AppBar(title: Text(c?.fullName ?? widget.name ?? l.t('patient'))),
      body: RefreshIndicator(
        onRefresh: _load,
        child: ListView(padding: const EdgeInsets.all(16), children: [
          if (c != null)
            Row(children: [
              PatientAvatar(c, size: 64),
              const SizedBox(width: 14),
              Expanded(
                child: Column(crossAxisAlignment: CrossAxisAlignment.start, children: [
                  Text(c.fullName, style: Theme.of(context).textTheme.titleLarge),
                  if (c.dateOfBirth != null) Text(l.f('born', [DateFormat('d MMMM y', l.language).format(DateTime.parse(c.dateOfBirth!))])),
                  if (c.guardianName != null) Text(l.f('guardian', [c.guardianName!])),
                  if (c.phone != null) Text(c.phone!),
                  if (c.email != null) Text(c.email!, style: TextStyle(color: Colors.grey[700])),
                ]),
              ),
            ]),
          const Divider(height: 32),
          Text(l.t('appointmentsWith'), style: Theme.of(context).textTheme.titleMedium),
          const SizedBox(height: 6),
          for (final a in _appointments)
            ListTile(
              contentPadding: EdgeInsets.zero,
              dense: true,
              title: Text(format.format(a.slotStart)),
              subtitle: (a.motif ?? '').isEmpty ? null : Text(a.motif!),
              trailing: AppointmentStatusChip(a.status, l.t(a.statusKey)),
            ),
          const Divider(height: 32),
          Text(l.t('documents'), style: Theme.of(context).textTheme.titleMedium),
          const SizedBox(height: 6),
          if (_error != null)
            ErrorBanner(_error!)
          else if (_documents == null)
            const Padding(padding: EdgeInsets.all(16), child: Center(child: CircularProgressIndicator()))
          else if (_documents!.isEmpty)
            Text(l.t('noDocuments'), style: TextStyle(color: Colors.grey[600]))
          else
            for (final d in _documents!)
              ListTile(
                contentPadding: EdgeInsets.zero,
                leading: Icon(d.contentType.startsWith('image/') ? Icons.image_outlined : Icons.description_outlined),
                title: Text(d.fileName, maxLines: 1, overflow: TextOverflow.ellipsis),
                subtitle: Text([d.category, if (d.createdAt != null) DateFormat('d MMM y', l.language).format(d.createdAt!.toLocal())].join(' · ')),
                trailing: const Icon(Icons.open_in_new, size: 18),
                onTap: () => _open(d),
              ),
        ]),
      ),
    );
  }
}
