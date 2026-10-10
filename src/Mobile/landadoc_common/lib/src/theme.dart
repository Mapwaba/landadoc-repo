import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';

// The web apps' palette (LandaDocTheme / landadoc-shell.css)
const brandBlue = Color(0xFF2563EB);
const brandText = Color(0xFF1E293B);
const brandSoft = Color(0xFFEEF4FF);

ThemeData landaDocTheme() => ThemeData(
      useMaterial3: true,
      colorScheme: ColorScheme.fromSeed(seedColor: brandBlue, primary: brandBlue),
      scaffoldBackgroundColor: const Color(0xFFF7F9FC),
      appBarTheme: const AppBarTheme(backgroundColor: Colors.white, foregroundColor: brandText, elevation: 0, scrolledUnderElevation: 1),
      inputDecorationTheme: const InputDecorationTheme(border: OutlineInputBorder(), isDense: true),
      filledButtonTheme: FilledButtonThemeData(
        style: FilledButton.styleFrom(minimumSize: const Size.fromHeight(48), shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10))),
      ),
    );

// French first, English available
const landaDocLocales = [Locale('fr'), Locale('en')];
const landaDocLocalizations = <LocalizationsDelegate<dynamic>>[
  GlobalMaterialLocalizations.delegate,
  GlobalWidgetsLocalizations.delegate,
  GlobalCupertinoLocalizations.delegate,
];
