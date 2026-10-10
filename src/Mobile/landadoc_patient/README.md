# LandaDoc — Patient mobile app (Flutter)

Replaced the .NET MAUI Patient.Mobile app (since removed). Built to stay light on low-cost Android phones and
expensive data: a handful of packages, doctor photos downloaded once and cached, a week of free
times per request, and the patient's appointments saved on the phone for offline reading.

## What it does

- **Sign in / create an account** (stays signed in for up to 7 days: the refresh token renews the
  15-minute access token). Patient accounts only.
- **Find a doctor** by name, specialty or city; photos come from `GET api/search/doctors/{id}/photo`
  and searches ask for `photos=false`.
- **Doctor profile → pick a day and time → book** for yourself or a family member. Free times come
  from `GET api/availability/slots/range` (14 days in one request). Times are the doctor's clock;
  when the phone's clock differs, "your time" is shown too (same rule as the web app).
- **Pay with Mobile Money** (M-Pesa, Orange, Airtel, Africell), then the screen follows the payment
  until it's confirmed. Card only outside the DRC (opens the secure card page). Insurance: website.
- **My appointments**, upcoming and past, readable offline.
- French by default, English available.

Family management, documents, address and photo stay on the website for now.

## Tools (on this machine)

Flutter and the Android SDK live in `D:\dev` (user environment: `PATH`, `ANDROID_HOME`,
`PUB_CACHE=D:\dev\pub-cache`, `GRADLE_USER_HOME=D:\dev\gradle`).

```
flutter pub get
flutter analyze
flutter test
flutter run                                  # on a connected phone or emulator
flutter build apk --release --split-per-abi  # APKs in build/app/outputs/flutter-apk/
```

Most phones need `app-arm64-v8a-release.apk`; older ones `app-armeabi-v7a-release.apk`.

## Server addresses

`lib/config.dart` defaults to the Render services. To use other ones (e.g. a local backend seen
from the Android emulator):

```
flutter run --dart-define=IDENTITY_URL=http://10.0.2.2:5138/ --dart-define=SEARCH_URL=http://10.0.2.2:5078/
```

## Still to do

- Push notifications (needs a Firebase project): booking confirmations, payment results, reminders.
- A proper release signing key (release builds currently use the debug key) and a launcher icon.
- Insurance payment, family management and documents (camera) in the app.
