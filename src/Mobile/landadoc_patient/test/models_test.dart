import 'package:flutter_test/flutter_test.dart';
import 'package:landadoc_patient/clock.dart';
import 'package:landadoc_patient/models.dart';

void main() {
  test('Appointment status arrives as a number from the Appointment service', () {
    final a = Appointment.fromJson({
      'id': 'a1', 'doctorId': 'd1', 'patientId': 'p1', 'refNumber': 42,
      'slotStart': '2026-10-12T09:30:00Z', 'status': 1, 'doctorName': 'Dr. Maz',
    });
    expect(a.status, AppointmentStatus.confirmed);
    expect(a.statusKey, 'status.Confirmed');
    expect(a.refNumber, 42);
  });

  test('A slot time is the doctor\'s wall clock, never shifted to the phone\'s zone', () {
    final t = wallClock('2026-10-12T09:30:00Z');
    expect([t.year, t.month, t.day, t.hour, t.minute], [2026, 10, 12, 9, 30]);
    expect(t.isUtc, isFalse);
  });

  test('Payment status arrives as text from the Payment service', () {
    final p = Payment.fromJson({'id': 'x', 'grossAmount': 20.0, 'status': 'Completed', 'provider': 'MokoAfrika'});
    expect(p.status, PaymentStatus.completed);
    expect(p.statusKey, 'payStatus.Completed');
  });

  test('A doctor without an inline photo still says whether one exists', () {
    final d = Doctor.fromJson({
      'doctorId': 'd1', 'firstName': 'Bruce', 'lastName': 'Maz', 'specialty': 'Generalist',
      'consultationFee': 180, 'averageRating': 5, 'ratingCount': 1, 'clinics': [], 'photoDataUrl': null, 'hasPhoto': true,
    });
    expect(d.hasPhoto, isTrue);
    expect(d.initials, 'BM');
    expect(d.fee, 180);
  });

  test('A week of free times is read day by day with the doctor\'s zone', () {
    final r = SlotRange.fromJson({
      'timeZone': 'Africa/Lubumbashi',
      'days': [
        {'date': '2026-10-12', 'slots': ['09:00', '09:30']},
        {'date': '2026-10-13', 'slots': []},
      ],
    });
    expect(r.timeZone, 'Africa/Lubumbashi');
    expect(r.days.first.slots, ['09:00', '09:30']);
    expect(r.days.last.slots, isEmpty);
    expect(cityOf(r.timeZone), 'Lubumbashi');
    expect(atSlot(r.days.first.date, '09:30').hour, 9);
  });

  test('Family and dependants are both offered as people to book for', () {
    final people = FamilyPerson.fromOverview({
      'family': [{'userId': 'u2', 'firstName': 'Marie', 'lastName': 'K', 'relationLabel': 'Parent'}],
      'dependents': [{'id': 'd9', 'firstName': 'Ruth', 'lastName': 'M', 'relationLabel': 'Child'}],
    });
    expect(people.map((p) => p.id), ['u2', 'd9']);
  });
}
