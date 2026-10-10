// Where the LandaDoc services answer. Defaults are the Render services the web apps use;
// override at build time, e.g. for a local backend:
//   flutter run --dart-define=IDENTITY_URL=http://10.0.2.2:5138/
// (10.0.2.2 is the computer itself, seen from the Android emulator.)
class ApiConfig {
  static const identity = String.fromEnvironment('IDENTITY_URL', defaultValue: 'https://landadoc-identity.onrender.com/');
  static const appointment = String.fromEnvironment('APPOINTMENT_URL', defaultValue: 'https://landadoc-appointment.onrender.com/');
  static const availability = String.fromEnvironment('AVAILABILITY_URL', defaultValue: 'https://landadoc-availability.onrender.com/');
  static const payment = String.fromEnvironment('PAYMENT_URL', defaultValue: 'https://landadoc-payment.onrender.com/');
  static const search = String.fromEnvironment('SEARCH_URL', defaultValue: 'https://landadoc-search.onrender.com/');

  // Render's free services can take up to a minute to wake up
  static const timeout = Duration(seconds: 70);
}
