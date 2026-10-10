import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:provider/provider.dart';

import 'l10n.dart';
import 'screens/home.dart';
import 'screens/login.dart';
import 'session.dart';

// LandaDoc for patients: find a doctor, book, pay with Mobile Money, follow appointments.
// Kept deliberately small for low-cost Android phones and expensive data: few packages, photos
// loaded once and cached, a week of free times per request, appointments saved for offline reading.
Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  final l10n = L10n();
  await l10n.load();
  final session = Session();
  runApp(MultiProvider(
    providers: [
      ChangeNotifierProvider.value(value: l10n),
      ChangeNotifierProvider.value(value: session),
    ],
    child: const LandaDocApp(),
  ));
  await session.restore();
}

// The web apps' palette (LandaDocTheme / landadoc-shell.css)
const brandBlue = Color(0xFF2563EB);
const brandText = Color(0xFF1E293B);
const brandSoft = Color(0xFFEEF4FF);

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
      supportedLocales: const [Locale('fr'), Locale('en')],
      localizationsDelegates: const [
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
      ],
      theme: ThemeData(
        useMaterial3: true,
        colorScheme: ColorScheme.fromSeed(seedColor: brandBlue, primary: brandBlue),
        scaffoldBackgroundColor: const Color(0xFFF7F9FC),
        appBarTheme: const AppBarTheme(backgroundColor: Colors.white, foregroundColor: brandText, elevation: 0, scrolledUnderElevation: 1),
        inputDecorationTheme: const InputDecorationTheme(border: OutlineInputBorder(), isDense: true),
        filledButtonTheme: FilledButtonThemeData(
          style: FilledButton.styleFrom(minimumSize: const Size.fromHeight(48), shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10))),
        ),
      ),
      home: session.loading
          ? const Scaffold(body: Center(child: CircularProgressIndicator()))
          : session.signedIn
              ? const HomeScreen()
              : const LoginScreen(),
    );
  }
}
