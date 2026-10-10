# LandaDoc — Doctor mobile app (Flutter)

Replaces the .NET MAUI Doctor.Mobile app with the daily work only; payments, payouts, rates,
the weekly schedule, the profile and registration (with ID documents) stay on the website.
Shares its server calls, sign-in, texts, widgets and logo with the Patient app through
`../landadoc_common`.

## What it does

- **Sign in** (doctor accounts only; new doctors register on the website). A banner says when the
  profile still waits for LandaDoc's approval.
- **Agenda**: from 3 days ago to 2 weeks ahead; each day's appointments with the patient's name
  (readable offline), **mark as completed**, open the patient file.
- **Time slots** of the day from the weekly schedule: open, booked or blocked. Tap an open slot to
  block it, a blocked one to reopen it, or block / reopen the whole day — uses
  `GET/POST api/availability/blocked/mine` and `POST api/availability/blocked/mine/unblock`.
- **Insurance claims** to review: approve with the insurer's reference, or decline with a reason;
  the approved ones still owed by insurers are listed below.
- **Patients**: search, patient file with photo, contact, appointments and documents (opened in
  the browser through a short-lived link).
- French by default, English available.

## Build

Same tools as the Patient app (`D:\dev`, see ../landadoc_patient/README.md):

```
flutter pub get
flutter analyze
flutter build apk --release --split-per-abi
```

## Still to do

- Push notifications (new booking, claim to review) — needs the Firebase project.
- Release signing key; Play Store listing.
