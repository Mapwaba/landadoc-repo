import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import '../l10n.dart';
import 'account.dart';
import 'appointments.dart';
import 'search.dart';

// Three tabs: find a doctor, my appointments, account. Tabs keep their state when switching.
class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int _tab = 0;
  final _appointmentsKey = GlobalKey<AppointmentsScreenState>();

  void showAppointments() {
    setState(() => _tab = 1);
    _appointmentsKey.currentState?.reload();
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    return Scaffold(
      body: IndexedStack(index: _tab, children: [
        SearchScreen(onBooked: showAppointments),
        AppointmentsScreen(key: _appointmentsKey),
        const AccountScreen(),
      ]),
      bottomNavigationBar: NavigationBar(
        selectedIndex: _tab,
        onDestinationSelected: (i) {
          setState(() => _tab = i);
          if (i == 1) _appointmentsKey.currentState?.reload();
        },
        destinations: [
          NavigationDestination(icon: const Icon(Icons.search), label: l.t('findDoctor')),
          NavigationDestination(icon: const Icon(Icons.event_note_outlined), selectedIcon: const Icon(Icons.event_note), label: l.t('myAppointments')),
          NavigationDestination(icon: const Icon(Icons.person_outline), selectedIcon: const Icon(Icons.person), label: l.t('account')),
        ],
      ),
    );
  }
}
