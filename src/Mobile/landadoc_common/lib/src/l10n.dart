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
    // ── Doctor app ──
    'doctorOnly': {'en': 'This app is for doctors. Patients use the LandaDoc Patient app.', 'fr': 'Cette application est réservée aux médecins. Les patients utilisent l\'application LandaDoc Patient.'},
    'patientOnly': {'en': 'This app is for patients. Doctors use the LandaDoc Doctor app.', 'fr': 'Cette application est réservée aux patients. Les médecins utilisent l\'application LandaDoc Médecin.'},
    'registerOnWeb': {'en': 'Opens the LandaDoc website, where you also add your ID and membership card.', 'fr': 'Ouvre le site LandaDoc, où vous ajoutez aussi votre pièce d\'identité et votre carte de membre.'},
    'awaitingApproval': {'en': 'Your account is waiting for LandaDoc to approve it. You can sign in, but patients can\'t book you yet.', 'fr': 'Votre compte attend la validation de LandaDoc. Vous pouvez vous connecter, mais les patients ne peuvent pas encore vous réserver.'},
    'noProfile': {'en': 'Finish your profile on the LandaDoc website first.', 'fr': 'Complétez d\'abord votre profil sur le site LandaDoc.'},
    'agenda': {'en': 'Agenda', 'fr': 'Agenda'},
    'claims': {'en': 'Claims', 'fr': 'Prises en charge'},
    'patients': {'en': 'Patients', 'fr': 'Patients'},
    'noAppointmentsDay': {'en': 'No appointments this day.', 'fr': 'Aucun rendez-vous ce jour.'},
    'patient': {'en': 'Patient', 'fr': 'Patient'},
    'markCompleted': {'en': 'Mark as completed', 'fr': 'Marquer comme terminé'},
    'completeQuestion': {'en': 'Mark this appointment as completed? The patient will be able to leave a review.', 'fr': 'Marquer ce rendez-vous comme terminé ? Le patient pourra laisser un avis.'},
    'confirm': {'en': 'Confirm', 'fr': 'Confirmer'},
    'cancel': {'en': 'Cancel', 'fr': 'Annuler'},
    'actionFailed': {'en': 'That didn\'t work. Please try again.', 'fr': 'Cela n\'a pas fonctionné. Veuillez réessayer.'},
    'openFile': {'en': 'Open patient file', 'fr': 'Ouvrir le dossier'},
    'slots': {'en': 'Time slots', 'fr': 'Créneaux'},
    'slotOpen': {'en': 'Open', 'fr': 'Libre'},
    'slotBooked': {'en': 'Booked', 'fr': 'Réservé'},
    'slotBlocked': {'en': 'Blocked', 'fr': 'Bloqué'},
    'slotsHint': {'en': 'Tap an open slot to block it, a blocked one to open it again.', 'fr': 'Touchez un créneau libre pour le bloquer, un créneau bloqué pour le rouvrir.'},
    'blockDay': {'en': 'Block the whole day', 'fr': 'Bloquer toute la journée'},
    'unblockDay': {'en': 'Open the whole day again', 'fr': 'Rouvrir toute la journée'},
    'notWorkingDay': {'en': 'You don\'t work this day (weekly schedule on the website).', 'fr': 'Vous ne travaillez pas ce jour (horaire hebdomadaire sur le site).'},
    'claimsToReview': {'en': 'To review', 'fr': 'À vérifier'},
    'claimsOwed': {'en': 'Owed by insurers', 'fr': 'Dus par les assureurs'},
    'noClaims': {'en': 'No claims to review.', 'fr': 'Aucune demande à vérifier.'},
    'member': {'en': 'Member no. {0}', 'fr': 'N° adhérent {0}'},
    'mainMember': {'en': 'Main member: {0}', 'fr': 'Adhérent principal : {0}'},
    'approve': {'en': 'Approve', 'fr': 'Approuver'},
    'decline': {'en': 'Decline', 'fr': 'Refuser'},
    'approveHint': {'en': 'Check the cover with the insurer first, then enter the reference they gave you.', 'fr': 'Vérifiez d\'abord la couverture auprès de l\'assureur, puis saisissez la référence qu\'il vous a donnée.'},
    'insurerReference': {'en': 'Insurer\'s reference (authorisation no. or agent\'s name)', 'fr': 'Référence de l\'assureur (n° d\'autorisation ou nom de l\'agent)'},
    'declineReason': {'en': 'Reason (optional)', 'fr': 'Motif (facultatif)'},
    'claimDone': {'en': 'This claim can no longer be changed.', 'fr': 'Cette demande ne peut plus être modifiée.'},
    'claim.Submitted': {'en': 'To review', 'fr': 'À vérifier'},
    'claim.Approved': {'en': 'Approved', 'fr': 'Approuvée'},
    'claim.Declined': {'en': 'Declined', 'fr': 'Refusée'},
    'claim.Settled': {'en': 'Paid by insurer', 'fr': 'Payée par l\'assureur'},
    'claim.Rejected': {'en': 'Rejected', 'fr': 'Rejetée'},
    'claim.Expired': {'en': 'Expired', 'fr': 'Expirée'},
    'noPatients': {'en': 'No patients yet.', 'fr': 'Pas encore de patients.'},
    'searchPatient': {'en': 'Search a patient', 'fr': 'Rechercher un patient'},
    'born': {'en': 'Born on {0}', 'fr': 'Né(e) le {0}'},
    'guardian': {'en': 'Guardian: {0}', 'fr': 'Tuteur : {0}'},
    'documents': {'en': 'Documents', 'fr': 'Documents'},
    'noDocuments': {'en': 'No documents.', 'fr': 'Aucun document.'},
    'appointmentsWith': {'en': 'Appointments', 'fr': 'Rendez-vous'},
    'webForDoctor': {'en': 'Payments, payouts, rates, your schedule and profile are managed on the LandaDoc website.', 'fr': 'Les paiements, versements, tarifs, votre horaire et votre profil se gèrent sur le site LandaDoc.'},
    'webForMore': {'en': 'Family, documents and your address are managed on the LandaDoc website for now.', 'fr': 'La famille, les documents et votre adresse se gèrent sur le site LandaDoc pour le moment.'},
  };
}
