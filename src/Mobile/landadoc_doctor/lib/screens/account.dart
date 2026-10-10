import 'package:flutter/material.dart';
import 'package:landadoc_common/landadoc_common.dart';
import 'package:provider/provider.dart';

class AccountScreen extends StatelessWidget {
  const AccountScreen({super.key});

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    final session = context.watch<Session>();
    final user = session.user;
    return Scaffold(
      appBar: AppBar(title: Text(l.t('account'))),
      body: ListView(padding: const EdgeInsets.all(16), children: [
        if (user != null) ...[
          Text('${l.t('dr')} ${user.fullName}', style: Theme.of(context).textTheme.titleLarge),
          Text(l.f('signedInAs', [user.email]), style: TextStyle(color: Colors.grey[700])),
          const SizedBox(height: 20),
        ],
        Text(l.t('language'), style: Theme.of(context).textTheme.titleMedium),
        const SizedBox(height: 8),
        const Align(alignment: Alignment.centerLeft, child: LanguageSwitch()),
        const SizedBox(height: 24),
        WebsiteNotice(l.t('webForDoctor'), ApiConfig.doctorWebsite),
        const SizedBox(height: 24),
        OutlinedButton.icon(
          onPressed: () => session.logout(),
          icon: const Icon(Icons.logout, color: Color(0xFFDC2626)),
          label: Text(l.t('logOut'), style: const TextStyle(color: Color(0xFFDC2626))),
        ),
      ]),
    );
  }
}
