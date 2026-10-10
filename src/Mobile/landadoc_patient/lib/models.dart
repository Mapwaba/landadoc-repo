// The parts of the LandaDoc API's answers the Patient app uses. Field names follow the
// services' JSON (camelCase). Enums arrive as text from some services and as numbers from
// others (e.g. Appointment), so both are accepted.

T _enumOf<T>(List<T> values, Object? raw, T fallback) {
  if (raw is int && raw >= 0 && raw < values.length) return values[raw];
  if (raw is String) {
    for (final v in values) {
      if ((v as Enum).name.toLowerCase() == raw.toLowerCase()) return v;
    }
  }
  return fallback;
}

double _num(Object? v) => v is num ? v.toDouble() : double.tryParse('$v') ?? 0;

// A slot or appointment time is wall-clock time on the doctor's clock, sent with a "Z" but not
// really UTC: read it as-is and never convert it to the phone's time zone.
DateTime wallClock(String value) {
  final d = DateTime.parse(value);
  return DateTime(d.year, d.month, d.day, d.hour, d.minute, d.second);
}

class UserProfile {
  final String id, email, role, firstName, lastName;
  final String? phone, country;

  UserProfile({required this.id, required this.email, required this.role, required this.firstName, required this.lastName, this.phone, this.country});

  factory UserProfile.fromJson(Map<String, dynamic> j) => UserProfile(
        id: j['id'] as String,
        email: j['email'] as String? ?? '',
        role: j['role'] as String? ?? '',
        firstName: j['firstName'] as String? ?? '',
        lastName: j['lastName'] as String? ?? '',
        phone: j['phone'] as String?,
        country: j['country'] as String?,
      );

  String get fullName => '$firstName $lastName'.trim();
}

class Clinic {
  final String id, name, city;
  final String? address;

  Clinic({required this.id, required this.name, required this.city, this.address});

  factory Clinic.fromJson(Map<String, dynamic> j) =>
      Clinic(id: j['id'] as String, name: j['name'] as String? ?? '', city: j['city'] as String? ?? '', address: j['address'] as String?);
}

class Doctor {
  final String id, firstName, lastName, specialty;
  final String? bio;
  final double fee, rating;
  final int ratingCount;
  final List<Clinic> clinics;
  final bool hasPhoto;

  Doctor({
    required this.id,
    required this.firstName,
    required this.lastName,
    required this.specialty,
    this.bio,
    required this.fee,
    required this.rating,
    required this.ratingCount,
    required this.clinics,
    required this.hasPhoto,
  });

  factory Doctor.fromJson(Map<String, dynamic> j) => Doctor(
        id: j['doctorId'] as String,
        firstName: j['firstName'] as String? ?? '',
        lastName: j['lastName'] as String? ?? '',
        specialty: j['specialty'] as String? ?? '',
        bio: j['bio'] as String?,
        fee: _num(j['consultationFee']),
        rating: _num(j['averageRating']),
        ratingCount: (j['ratingCount'] as num?)?.toInt() ?? 0,
        clinics: ((j['clinics'] as List?) ?? []).map((c) => Clinic.fromJson(c as Map<String, dynamic>)).toList(),
        hasPhoto: j['hasPhoto'] as bool? ?? (j['photoDataUrl'] != null),
      );

  String get initials => '${firstName.isNotEmpty ? firstName[0] : ''}${lastName.isNotEmpty ? lastName[0] : ''}'.toUpperCase();
}

class DaySlots {
  final DateTime date;
  final List<String> slots;

  DaySlots(this.date, this.slots);

  factory DaySlots.fromJson(Map<String, dynamic> j) =>
      DaySlots(DateTime.parse(j['date'] as String), ((j['slots'] as List?) ?? []).cast<String>());
}

class SlotRange {
  final List<DaySlots> days;
  final String? timeZone;

  SlotRange(this.days, this.timeZone);

  factory SlotRange.fromJson(Map<String, dynamic> j) =>
      SlotRange(((j['days'] as List?) ?? []).map((d) => DaySlots.fromJson(d as Map<String, dynamic>)).toList(), j['timeZone'] as String?);
}

enum AppointmentStatus { pending, confirmed, completed, cancelled, rescheduled }

class Appointment {
  final String id, doctorId, patientId;
  final int refNumber;
  final DateTime slotStart;
  final AppointmentStatus status;
  final String? motif, doctorName, specialty, patientName, clinicName, bookedByUserId;

  Appointment({
    required this.id,
    required this.doctorId,
    required this.patientId,
    required this.refNumber,
    required this.slotStart,
    required this.status,
    this.motif,
    this.doctorName,
    this.specialty,
    this.patientName,
    this.clinicName,
    this.bookedByUserId,
  });

  factory Appointment.fromJson(Map<String, dynamic> j) => Appointment(
        id: j['id'] as String,
        doctorId: j['doctorId'] as String,
        patientId: j['patientId'] as String,
        refNumber: (j['refNumber'] as num?)?.toInt() ?? 0,
        slotStart: wallClock(j['slotStart'] as String),
        status: _enumOf(AppointmentStatus.values, j['status'], AppointmentStatus.pending),
        motif: j['motif'] as String?,
        doctorName: j['doctorName'] as String?,
        specialty: j['specialty'] as String?,
        patientName: j['patientName'] as String?,
        clinicName: j['clinicName'] as String?,
        bookedByUserId: j['bookedByUserId'] as String?,
      );

  String get statusKey => 'status.${status.name[0].toUpperCase()}${status.name.substring(1)}';
}

enum PaymentStatus { pending, completed, refunded, failed }

class Payment {
  final String id;
  final double gross;
  final PaymentStatus status;
  final String? provider;

  Payment({required this.id, required this.gross, required this.status, this.provider});

  factory Payment.fromJson(Map<String, dynamic> j) => Payment(
        id: j['id'] as String,
        gross: _num(j['grossAmount']),
        status: _enumOf(PaymentStatus.values, j['status'], PaymentStatus.pending),
        provider: j['provider']?.toString(),
      );

  String get statusKey => 'payStatus.${status.name[0].toUpperCase()}${status.name.substring(1)}';
}

// Someone the signed-in user can book for: a linked adult (family) or a dependant (a child)
class FamilyPerson {
  final String id, name, relation;

  FamilyPerson(this.id, this.name, this.relation);

  static List<FamilyPerson> fromOverview(Map<String, dynamic> j) => [
        ...((j['family'] as List?) ?? []).map((m) => FamilyPerson(m['userId'] as String, '${m['firstName']} ${m['lastName']}', m['relationLabel'] as String? ?? '')),
        ...((j['dependents'] as List?) ?? []).map((d) => FamilyPerson(d['id'] as String, '${d['firstName']} ${d['lastName']}', d['relationLabel'] as String? ?? '')),
      ];
}
