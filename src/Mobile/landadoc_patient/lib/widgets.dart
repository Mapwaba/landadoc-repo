import 'package:cached_network_image/cached_network_image.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

import 'api.dart';
import 'l10n.dart';
import 'models.dart';

// The website's logo (cross + "LandaDoc — doctor on time!"), at the top of login and register
class BrandHeader extends StatelessWidget {
  final double width;
  const BrandHeader({this.width = 240, super.key});

  @override
  Widget build(BuildContext context) => Center(
        child: Image.asset(
          'assets/landadoc-logo.png',
          width: width,
          filterQuality: FilterQuality.high,
          semanticLabel: 'LandaDoc',
        ),
      );
}

class ErrorBanner extends StatelessWidget {
  final String message;
  const ErrorBanner(this.message, {super.key});

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.all(12),
        decoration: BoxDecoration(color: const Color(0xFFFEF2F2), borderRadius: BorderRadius.circular(8)),
        child: Row(children: [
          const Icon(Icons.error_outline, color: Color(0xFFDC2626), size: 20),
          const SizedBox(width: 8),
          Expanded(child: Text(message, style: const TextStyle(color: Color(0xFF991B1B)))),
        ]),
      );
}

class InfoBanner extends StatelessWidget {
  final String message;
  final IconData icon;
  const InfoBanner(this.message, {this.icon = Icons.info_outline, super.key});

  @override
  Widget build(BuildContext context) => Container(
        padding: const EdgeInsets.all(12),
        decoration: BoxDecoration(color: const Color(0xFFEEF4FF), borderRadius: BorderRadius.circular(8)),
        child: Row(children: [
          Icon(icon, color: Theme.of(context).colorScheme.primary, size: 20),
          const SizedBox(width: 8),
          Expanded(child: Text(message)),
        ]),
      );
}

class ButtonSpinner extends StatelessWidget {
  const ButtonSpinner({super.key});

  @override
  Widget build(BuildContext context) => const SizedBox(width: 20, height: 20, child: CircularProgressIndicator(strokeWidth: 2.5, color: Colors.white));
}

// The doctor's photo, downloaded once and kept on the phone; their initials until then or without one
class DoctorAvatar extends StatelessWidget {
  final Doctor doctor;
  final double size;
  const DoctorAvatar(this.doctor, {this.size = 48, super.key});

  @override
  Widget build(BuildContext context) {
    final scheme = Theme.of(context).colorScheme;
    final initials = CircleAvatar(
      radius: size / 2,
      backgroundColor: const Color(0xFFEEF4FF),
      child: Text(doctor.initials, style: TextStyle(color: scheme.primary, fontWeight: FontWeight.w700, fontSize: size / 3)),
    );
    if (!doctor.hasPhoto) return initials;
    return ClipOval(
      child: CachedNetworkImage(
        imageUrl: Api.photoUrl(doctor.id),
        width: size,
        height: size,
        fit: BoxFit.cover,
        memCacheWidth: (size * 3).round(),   // decode small: saves memory on low-end phones
        placeholder: (_, _) => initials,
        errorWidget: (_, _, _) => initials,
      ),
    );
  }
}

class LanguageSwitch extends StatelessWidget {
  const LanguageSwitch({super.key});

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    return Center(
      child: SegmentedButton<String>(
        segments: const [ButtonSegment(value: 'fr', label: Text('FR')), ButtonSegment(value: 'en', label: Text('EN'))],
        selected: {l.language},
        onSelectionChanged: (s) => l.setLanguage(s.first),
        showSelectedIcon: false,
      ),
    );
  }
}

// "Something went wrong" with a retry button, for lists that failed to load
class RetryPanel extends StatelessWidget {
  final String message;
  final VoidCallback onRetry;
  const RetryPanel(this.message, this.onRetry, {super.key});

  @override
  Widget build(BuildContext context) {
    final l = context.watch<L10n>();
    return Center(
      child: Padding(
        padding: const EdgeInsets.all(24),
        child: Column(mainAxisSize: MainAxisSize.min, children: [
          Icon(Icons.wifi_off_rounded, size: 40, color: Colors.grey[500]),
          const SizedBox(height: 12),
          Text(message, textAlign: TextAlign.center),
          const SizedBox(height: 12),
          OutlinedButton(onPressed: onRetry, child: Text(l.t('retry'))),
        ]),
      ),
    );
  }
}

String money(double amount) => '\$${amount.toStringAsFixed(amount == amount.roundToDouble() ? 0 : 2)}';
