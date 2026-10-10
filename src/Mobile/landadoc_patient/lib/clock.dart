// Appointment times are wall-clock times on the doctor's clock (where they work). The DRC has
// two zones; the phone may be elsewhere (a relative booking from abroad). Like the web app, the
// phone's own time is shown next to the doctor's when the two clocks differ.
const _offsets = {'Africa/Kinshasa': Duration(hours: 1), 'Africa/Lubumbashi': Duration(hours: 2)};

String cityOf(String? zone) => (zone ?? 'Africa/Kinshasa').split('/').last.replaceAll('_', ' ');

// The phone's time for a slot on the doctor's clock, or null when both clocks read the same.
// Unknown doctor zones are taken as Kinshasa, like the server does.
DateTime? onPhoneClock(DateTime doctorWallClock, String? zone) {
  final doctorOffset = _offsets[zone] ?? _offsets['Africa/Kinshasa']!;
  final utc = DateTime.utc(doctorWallClock.year, doctorWallClock.month, doctorWallClock.day,
          doctorWallClock.hour, doctorWallClock.minute)
      .subtract(doctorOffset);
  final phone = utc.toLocal();
  if (phone.timeZoneOffset == doctorOffset) return null;
  return DateTime(phone.year, phone.month, phone.day, phone.hour, phone.minute);
}

String hhmm(DateTime t) => '${t.hour.toString().padLeft(2, '0')}:${t.minute.toString().padLeft(2, '0')}';

DateTime atSlot(DateTime day, String slot) {
  final parts = slot.split(':');
  return DateTime(day.year, day.month, day.day, int.parse(parts[0]), int.parse(parts[1]));
}
