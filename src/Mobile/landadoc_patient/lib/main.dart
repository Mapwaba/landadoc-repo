import 'package:flutter/material.dart';
import 'package:landadoc_common/landadoc_common.dart';
import 'package:provider/provider.dart';

import 'screens/home.dart';
import 'screens/login.dart';

// LandaDoc for patients: find a doctor, book, pay with Mobile Money, follow appointments.
// Kept deliberately small for low-cost Android phones and expensive data: few packages, photos
// loaded once and cached, a week of free times per request, appointments saved for offline reading.
// What it shares with the Doctor app (server calls, sign-in, texts, widgets) is in landadoc_common.
Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  final l10n = L10n();
  await l10n.load();
  final session = Session(role: 'Patient');
  runApp(MultiProvider(
    providers: [
      ChangeNotifierProvider.value(value: l10n),
      ChangeNotifierProvider.value(value: session),
    ],
    child: const LandaDocApp(),
  ));
  await session.restore();
}

class LandaDocApp extends StatelessWidget {
  const LandaDocApp({super.key});

  @override
  Widget build(BuildContext context) {
    final l10n = context.watch<L10n>();
    final session = context.watch<Session>();
    return MaterialApp(
      title: 'LandaDoc',
      debugShowCheckedModeBanner: false,
      locale: Locale(l10n.language),
      supportedLocales: landaDocLocales,
      localizationsDelegates: landaDocLocalizations,
      theme: landaDocTheme(),
      home: session.loading
          ? const Scaffold(body: Center(child: CircularProgressIndicator()))
          : session.signedIn
              ? const HomeScreen()
              : const LoginScreen(),
    );
  }
}
