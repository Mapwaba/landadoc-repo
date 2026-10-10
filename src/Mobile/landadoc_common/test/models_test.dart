import 'package:flutter_test/flutter_test.dart';
import 'package:landadoc_common/landadoc_common.dart';

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

  test('A weekly schedule day gives its time slots, like the server does', () {
    final day = ScheduleDay.fromJson({'day': 'Monday', 'openTime': '09:00:00', 'closeTime': '11:00:00', 'slotMinutes': 30});
    expect(day.weekday, DateTime.monday);
    expect(day.slots(), ['09:00', '09:30', '10:00', '10:30']);
    final late = ScheduleDay.fromJson({'day': 6, 'openTime': '22:00:00', 'closeTime': '23:59:00', 'slotMinutes': 30});
    expect(late.weekday, DateTime.sunday);
    expect(late.slots(), ['22:00', '22:30', '23:00']);   // no wrap past midnight
  });

  test('A claim to review is read with its insurer and status', () {
    final c = InsuranceClaim.fromJson({
      'id': 'c1', 'appointmentId': 'a1', 'patientId': 'p1', 'insurerName': 'SONAS', 'memberNumber': 'M-77',
      'amount': 50.0, 'status': 'Submitted', 'createdAt': '2026-10-10T08:00:00Z',
    });
    expect(c.status, ClaimStatus.submitted);
    expect(c.statusKey, 'claim.Submitted');
    expect(c.amount, 50);
  });

  test('Slot times go to the server as wall-clock times with Z', () {
    expect(Api.wallParam(DateTime(2026, 10, 12, 9, 5)), '2026-10-12T09:05:00Z');
    expect(Api.dateParam(DateTime(2026, 1, 3)), '2026-01-03');
  });

  test('Family and dependants are both offered as people to book for', () {
    final people = FamilyPerson.fromOverview({
      'family': [{'userId': 'u2', 'firstName': 'Marie', 'lastName': 'K', 'relationLabel': 'Parent'}],
      'dependents': [{'id': 'd9', 'firstName': 'Ruth', 'lastName': 'M', 'relationLabel': 'Child'}],
    });
    expect(people.map((p) => p.id), ['u2', 'd9']);
  });
}
