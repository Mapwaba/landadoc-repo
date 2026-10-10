import 'package:flutter/foundation.dart';
import 'package:shared_preferences/shared_preferences.dart';

// The app's texts in French (default) and English, like the web apps' Translations.
// A missing key shows the key itself, so nothing breaks while a text is being added.
class L10n extends ChangeNotifier {
  static const _storageKey = 'landadoc_language';
  String _language = 'fr';

  String get language => _language;

  Future<void> load() async {
    final prefs = await SharedPreferences.getInstance();
    _language = prefs.getString(_storageKey) ?? 'fr';
  }

  Future<void> setLanguage(String language) async {
    _language = language;
    notifyListeners();
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString(_storageKey, language);
  }

  String t(String key) => _texts[key]?[_language] ?? _texts[key]?['en'] ?? key;

  // "{0}" placeholders, like string.Format on the web
  String f(String key, List<Object> args) {
    var text = t(key);
    for (var i = 0; i < args.length; i++) {
      text = text.replaceAll('{$i}', '${args[i]}');
    }
    return text;
  }

  static const Map<String, Map<String, String>> _texts = {
    'appName': {'en': 'LandaDoc', 'fr': 'LandaDoc'},
    'tagline': {'en': 'Doctor on time', 'fr': 'Le médecin à temps'},
    // Sign in / up
    'email': {'en': 'Email', 'fr': 'Email'},
    'password': {'en': 'Password', 'fr': 'Mot de passe'},
    'confirmPassword': {'en': 'Confirm password', 'fr': 'Confirmer le mot de passe'},
    'logIn': {'en': 'Log in', 'fr': 'Se connecter'},
    'logOut': {'en': 'Log out', 'fr': 'Se déconnecter'},
    'register': {'en': 'Create an account', 'fr': 'Créer un compte'},
    'noAccount': {'en': "Don't have an account?", 'fr': "Vous n'avez pas de compte ?"},
    'haveAccount': {'en': 'Already have an account?', 'fr': 'Vous avez déjà un compte ?'},
    'firstName': {'en': 'First name', 'fr': 'Prénom'},
    'lastName': {'en': 'Last name', 'fr': 'Nom'},
    'phone': {'en': 'Phone', 'fr': 'Téléphone'},
    'idNumber': {'en': 'ID card or passport number', 'fr': "Numéro de carte d'identité ou de passeport"},
    'country': {'en': 'Country of residence', 'fr': 'Pays de résidence'},
    'countryDrc': {'en': 'DR Congo', 'fr': 'RD Congo'},
    'countryOther': {'en': 'Another country', 'fr': 'Un autre pays'},
    'required': {'en': 'Required', 'fr': 'Obligatoire'},
    'passwordTooShort': {'en': 'At least 8 characters', 'fr': 'Au moins 8 caractères'},
    'passwordsDiffer': {'en': "The passwords don't match.", 'fr': 'Les mots de passe ne correspondent pas.'},
    'wrongLogin': {'en': 'Wrong email or password.', 'fr': 'Email ou mot de passe incorrect.'},
    'registerFailed': {'en': 'Registration failed. Please check your details.', 'fr': "L'inscription a échoué. Vérifiez vos informations."},
    'networkError': {'en': "Couldn't reach LandaDoc. Check your connection and try again.", 'fr': 'Impossible de joindre LandaDoc. Vérifiez votre connexion et réessayez.'},
    'retry': {'en': 'Try again', 'fr': 'Réessayer'},
    // Navigation
    'findDoctor': {'en': 'Find a doctor', 'fr': 'Trouver un médecin'},
    'myAppointments': {'en': 'My appointments', 'fr': 'Mes rendez-vous'},
    'account': {'en': 'Account', 'fr': 'Compte'},
    // Search
    'searchHint': {'en': 'Doctor, specialty or city', 'fr': 'Médecin, spécialité ou ville'},
    'noDoctors': {'en': 'No doctor matches your search.', 'fr': 'Aucun médecin ne correspond à votre recherche.'},
    'dr': {'en': 'Dr.', 'fr': 'Dr'},
    'fee': {'en': 'Consultation', 'fr': 'Consultation'},
    'reviews': {'en': '{0} reviews', 'fr': '{0} avis'},
    // Doctor & booking
    'about': {'en': 'About', 'fr': 'À propos'},
    'availableTimes': {'en': 'Available times', 'fr': 'Créneaux disponibles'},
    'noSlotsDay': {'en': 'No open slots this day.', 'fr': 'Aucun créneau ouvert ce jour.'},
    'today': {'en': 'Today', 'fr': "Aujourd'hui"},
    'bookFor': {'en': 'Appointment for', 'fr': 'Rendez-vous pour'},
    'myself': {'en': 'Myself', 'fr': 'Moi-même'},
    'reason': {'en': 'Reason for the visit (optional)', 'fr': 'Motif de la consultation (facultatif)'},
    'book': {'en': 'Book', 'fr': 'Réserver'},
    'slotTaken': {'en': 'That slot was just taken. Please pick another time.', 'fr': 'Ce créneau vient d\'être pris. Choisissez un autre horaire.'},
    'bookFailed': {'en': "The booking couldn't be made. Please try again.", 'fr': "La réservation n'a pas pu être faite. Veuillez réessayer."},
    'zoneNote': {'en': 'Times are {0} time, where the doctor works.', 'fr': 'Les heures sont celles de {0}, où exerce le médecin.'},
    'yourTime': {'en': '{0} your time', 'fr': '{0} chez vous'},
    // Appointments
    'noAppointments': {'en': "You don't have any appointments yet.", 'fr': "Vous n'avez pas encore de rendez-vous."},
    'upcoming': {'en': 'Upcoming', 'fr': 'À venir'},
    'past': {'en': 'Past', 'fr': 'Passés'},
    'status.Pending': {'en': 'Awaiting payment', 'fr': 'En attente de paiement'},
    'status.Confirmed': {'en': 'Confirmed', 'fr': 'Confirmé'},
    'status.Completed': {'en': 'Completed', 'fr': 'Terminé'},
    'status.Cancelled': {'en': 'Cancelled', 'fr': 'Annulé'},
    'status.Rescheduled': {'en': 'Rescheduled', 'fr': 'Reporté'},
    'offlineCopy': {'en': 'Offline — showing your last saved appointments.', 'fr': 'Hors ligne — affichage de vos derniers rendez-vous enregistrés.'},
    'appointment': {'en': 'Appointment', 'fr': 'Rendez-vous'},
    'ref': {'en': 'Ref. #{0}', 'fr': 'Réf. n°{0}'},
    'for': {'en': 'For {0}', 'fr': 'Pour {0}'},
    // Payment
    'payment': {'en': 'Payment', 'fr': 'Paiement'},
    'total': {'en': 'Total', 'fr': 'Total'},
    'paid': {'en': 'Paid', 'fr': 'Payé'},
    'payStatus.Pending': {'en': 'To pay', 'fr': 'À payer'},
    'payStatus.Completed': {'en': 'Paid', 'fr': 'Payé'},
    'payStatus.Failed': {'en': 'Failed', 'fr': 'Échoué'},
    'payStatus.Refunded': {'en': 'Refunded', 'fr': 'Remboursé'},
    'mobileMoney': {'en': 'Mobile Money', 'fr': 'Mobile Money'},
    'card': {'en': 'Card', 'fr': 'Carte'},
    'fullName': {'en': 'Full name', 'fr': 'Nom complet'},
    'mobileNumber': {'en': 'Mobile money number', 'fr': 'Numéro mobile money'},
    'operator': {'en': 'Operator', 'fr': 'Opérateur'},
    'payNow': {'en': 'Pay now', 'fr': 'Payer maintenant'},
    'checkPhone': {'en': 'Check your phone to approve the payment.', 'fr': 'Vérifiez votre téléphone pour approuver le paiement.'},
    'airtelFee': {'en': 'Airtel charges an extra 2% on your account.', 'fr': 'Airtel facture 2 % de frais supplémentaires sur votre compte.'},
    'payFailed': {'en': "The payment couldn't start.", 'fr': "Le paiement n'a pas pu démarrer."},
    'paymentFailedRetry': {'en': 'The payment failed. You can try again.', 'fr': 'Le paiement a échoué. Vous pouvez réessayer.'},
    'insuranceOnWeb': {'en': 'To pay through your insurer, use the LandaDoc website for now.', 'fr': 'Pour passer par votre assureur, utilisez le site LandaDoc pour le moment.'},
    // Account
    'language': {'en': 'Language', 'fr': 'Langue'},
    'signedInAs': {'en': 'Signed in as {0}', 'fr': 'Connecté en tant que {0}'},
    'webForMore': {'en': 'Family, documents and your address are managed on the LandaDoc website for now.', 'fr': 'La famille, les documents et votre adresse se gèrent sur le site LandaDoc pour le moment.'},
  };
}
