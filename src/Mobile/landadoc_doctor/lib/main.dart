import 'package:flutter/material.dart';
import 'package:landadoc_common/landadoc_common.dart';
import 'package:provider/provider.dart';

import 'screens/home.dart';
import 'screens/login.dart';

// LandaDoc for doctors: the day's appointments, blocking time, insurance claims to review and
// patient files. Payments, payouts, rates, the weekly schedule and the profile stay on the website.
// What it shares with the Patient app (server calls, sign-in, texts, widgets) is in landadoc_common.
Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  final l10n = L10n();
  await l10n.load();
  final session = Session(role: 'Doctor');
  runApp(MultiProvider(
    providers: [
      ChangeNotifierProvider.value(value: l10n),
      ChangeNotifierProvider.value(value: session),
    ],
    child: const LandaDocDoctorApp(),
  ));
  await session.restore();
}

class LandaDocDoctorApp extends StatelessWidget {
  const LandaDocDoctorApp({super.key});

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
