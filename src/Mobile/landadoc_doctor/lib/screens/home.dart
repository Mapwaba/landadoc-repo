import 'package:flutter/material.dart';
import 'package:landadoc_common/landadoc_common.dart';
import 'package:provider/provider.dart';

import 'account.dart';
import 'agenda.dart';
import 'claims.dart';
import 'patients.dart';

// Four tabs: agenda, insurance claims, patients, account. A banner says when the profile still
// waits for LandaDoc's approval (the doctor can use the app, but patients can't book them yet).
class HomeScreen extends StatefulWidget {
  const HomeScreen({super.key});

  @override
  State<HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<HomeScreen> {
  int _tab = 0;
  String? _profileNotice;
  final _claimsKey = GlobalKey<ClaimsScreenState>();

  @override
  void initState() {
    super.initState();
    _checkProfile();
  }

  Future<void> _checkProfile() async {
    final l = context.read<L10n>();
    try {
      final profile = await context.read<Session>().api.myDoctorProfile();
      if (!mounted) return;
      final status = profile?['status']?.toString();
      setState(() => _profileNotice = profile == null
          ? l.t('noProfile')
          : status == 'Pending' || status == '0'
              ? l.t('awaitingApproval')
              : null);
    } catch (_) {
      // not essential: without it there's simply no banner
    }
  }

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    return Scaffold(
      body: Column(children: [
        if (_profileNotice != null)
          SafeArea(bottom: false, child: Padding(padding: const EdgeInsets.fromLTRB(12, 8, 12, 0), child: InfoBanner(_profileNotice!, icon: Icons.hourglass_top))),
        Expanded(
          child: IndexedStack(index: _tab, children: [
            const AgendaScreen(),
            ClaimsScreen(key: _claimsKey),
            const PatientsScreen(),
            const AccountScreen(),
          ]),
        ),
      ]),
      bottomNavigationBar: NavigationBar(
        selectedIndex: _tab,
        onDestinationSelected: (i) {
          setState(() => _tab = i);
          if (i == 1) _claimsKey.currentState?.reload();
        },
        destinations: [
          NavigationDestination(icon: const Icon(Icons.calendar_month_outlined), selectedIcon: const Icon(Icons.calendar_month), label: l.t('agenda')),
          NavigationDestination(icon: const Icon(Icons.health_and_safety_outlined), selectedIcon: const Icon(Icons.health_and_safety), label: l.t('claims')),
          NavigationDestination(icon: const Icon(Icons.people_outline), selectedIcon: const Icon(Icons.people), label: l.t('patients')),
          NavigationDestination(icon: const Icon(Icons.person_outline), selectedIcon: const Icon(Icons.person), label: l.t('account')),
        ],
      ),
    );
  }
}
