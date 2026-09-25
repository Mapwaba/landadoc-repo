namespace LandaDoc.Frontend.Shared.Services;

// Keys are the source English text itself wherever the string has no embedded dynamic
// data — that avoids inventing a parallel naming scheme for hundreds of one-off labels.
// Strings with interpolated/dynamic content use a short semantic key with {0}, {1}...
// composite-format placeholders instead, since the English text itself can't serve as a
// stable key once it contains runtime values.
public static class Translations
{
    public static readonly Dictionary<string, Dictionary<string, string>> Map = new()
    {
        // ── Shared nav / layout ─────────────────────────────────────────
        ["Checking session…"] = new() { ["en"] = "Checking session…", ["fr"] = "Vérification de la session…" },
        ["Not found"] = new() { ["en"] = "Not found", ["fr"] = "Introuvable" },
        ["Sorry, there's nothing at this address."] = new() { ["en"] = "Sorry, there's nothing at this address.", ["fr"] = "Désolé, il n'y a rien à cette adresse." },
        ["LandaDoc for Doctors"] = new() { ["en"] = "LandaDoc for Doctors", ["fr"] = "LandaDoc pour les médecins" },
        ["LandaDoc Admin"] = new() { ["en"] = "LandaDoc Admin", ["fr"] = "LandaDoc Admin" },
        ["Find a Doctor"] = new() { ["en"] = "Find a Doctor", ["fr"] = "Trouver un médecin" },
        ["My Appointments"] = new() { ["en"] = "My Appointments", ["fr"] = "Mes rendez-vous" },
        ["Documents"] = new() { ["en"] = "Documents", ["fr"] = "Documents" },
        ["Notifications"] = new() { ["en"] = "Notifications", ["fr"] = "Notifications" },
        ["Log out"] = new() { ["en"] = "Log out", ["fr"] = "Se déconnecter" },
        ["Log in"] = new() { ["en"] = "Log in", ["fr"] = "Se connecter" },
        ["Register"] = new() { ["en"] = "Register", ["fr"] = "S'inscrire" },
        ["Appointments"] = new() { ["en"] = "Appointments", ["fr"] = "Rendez-vous" },
        ["Schedule"] = new() { ["en"] = "Schedule", ["fr"] = "Horaire" },
        ["Profile"] = new() { ["en"] = "Profile", ["fr"] = "Profil" },
        ["Reviews"] = new() { ["en"] = "Reviews", ["fr"] = "Avis" },
        ["Overview"] = new() { ["en"] = "Overview", ["fr"] = "Aperçu" },
        ["Doctors"] = new() { ["en"] = "Doctors", ["fr"] = "Médecins" },
        ["Clinics"] = new() { ["en"] = "Clinics", ["fr"] = "Cliniques" },

        // ── Patient: Home ───────────────────────────────────────────────
        ["Welcome to LandaDoc"] = new() { ["en"] = "Welcome to LandaDoc", ["fr"] = "Bienvenue sur LandaDoc" },
        ["Find a doctor, see their availability, and book an appointment — all in one place."] = new()
        {
            ["en"] = "Find a doctor, see their availability, and book an appointment — all in one place.",
            ["fr"] = "Trouvez un médecin, consultez ses disponibilités et prenez rendez-vous — le tout au même endroit.",
        },
        ["Patients & Doctors"] = new() { ["en"] = "Patients & Doctors", ["fr"] = "Patients et médecins" },
        ["Patients"] = new() { ["en"] = "Patients", ["fr"] = "Patients" },
        ["Appointments by doctor"] = new() { ["en"] = "Appointments by doctor", ["fr"] = "Rendez-vous par médecin" },
        ["Click a doctor or a slice to see their availability"] = new()
        {
            ["en"] = "Click a doctor or a slice to see their availability",
            ["fr"] = "Cliquez sur un médecin ou une part du graphique pour voir ses disponibilités",
        },
        ["No appointments yet."] = new() { ["en"] = "No appointments yet.", ["fr"] = "Aucun rendez-vous pour le moment." },
        ["totalAppointmentsCount"] = new() { ["en"] = "{0} total appointments", ["fr"] = "{0} rendez-vous au total" },
        ["Find a doctor"] = new() { ["en"] = "Find a doctor", ["fr"] = "Trouver un médecin" },
        ["Specialty"] = new() { ["en"] = "Specialty", ["fr"] = "Spécialité" },
        ["City"] = new() { ["en"] = "City", ["fr"] = "Ville" },
        ["Search by name"] = new() { ["en"] = "Search by name", ["fr"] = "Rechercher par nom" },
        ["No doctors found matching your search."] = new()
        {
            ["en"] = "No doctors found matching your search.",
            ["fr"] = "Aucun médecin ne correspond à votre recherche.",
        },
        ["Dr."] = new() { ["en"] = "Dr.", ["fr"] = "Dr" },
        ["reviewCount"] = new() { ["en"] = "{0} review{1}", ["fr"] = "{0} avis" },
        ["View profile"] = new() { ["en"] = "View profile", ["fr"] = "Voir le profil" },
        ["Easy access"] = new() { ["en"] = "Easy access", ["fr"] = "Accès simple" },
        ["Find all your practitioners and history in one click."] = new()
        {
            ["en"] = "Find all your practitioners and history in one click.",
            ["fr"] = "Retrouvez tous vos praticiens et historiques en un clic.",
        },
        ["Book anytime, 24/7"] = new() { ["en"] = "Book anytime, 24/7", ["fr"] = "Prise de RDV 24/7" },
        ["Book whenever, wherever you are, without waiting."] = new()
        {
            ["en"] = "Book whenever, wherever you are, without waiting.",
            ["fr"] = "Réservez à tout moment, où que vous soyez, sans attendre.",
        },
        ["SMS reminders"] = new() { ["en"] = "SMS reminders", ["fr"] = "Rappels SMS" },
        ["Get automatic reminders so you never miss an appointment again."] = new()
        {
            ["en"] = "Get automatic reminders so you never miss an appointment again.",
            ["fr"] = "Recevez des rappels automatiques et ne manquez plus jamais vos rendez-vous.",
        },

        // ── Admin: Home ─────────────────────────────────────────────────
        ["Admin dashboard"] = new() { ["en"] = "Admin dashboard", ["fr"] = "Tableau de bord administrateur" },
        ["Manage doctors, clinics, and keep an eye on platform activity."] = new()
        {
            ["en"] = "Manage doctors, clinics, and keep an eye on platform activity.",
            ["fr"] = "Gérez les médecins, les cliniques et suivez l'activité de la plateforme.",
        },
        ["Manage doctors"] = new() { ["en"] = "Manage doctors", ["fr"] = "Gérer les médecins" },
        ["Review pending approvals and manage doctor profiles."] = new()
        {
            ["en"] = "Review pending approvals and manage doctor profiles.",
            ["fr"] = "Examinez les demandes en attente et gérez les profils des médecins.",
        },
        ["Manage clinics"] = new() { ["en"] = "Manage clinics", ["fr"] = "Gérer les cliniques" },
        ["Add, edit, or remove clinic locations."] = new()
        {
            ["en"] = "Add, edit, or remove clinic locations.",
            ["fr"] = "Ajoutez, modifiez ou supprimez des lieux de clinique.",
        },
        ["Go"] = new() { ["en"] = "Go", ["fr"] = "Accéder" },

        // ── Auth: shared across apps ────────────────────────────────────
        ["Email"] = new() { ["en"] = "Email", ["fr"] = "E-mail" },
        ["Password"] = new() { ["en"] = "Password", ["fr"] = "Mot de passe" },
        ["No account yet?"] = new() { ["en"] = "No account yet?", ["fr"] = "Pas encore de compte ?" },
        ["First name"] = new() { ["en"] = "First name", ["fr"] = "Prénom" },
        ["Last name"] = new() { ["en"] = "Last name", ["fr"] = "Nom" },
        ["Password is required"] = new() { ["en"] = "Password is required", ["fr"] = "Le mot de passe est requis" },
        ["At least 8 characters"] = new() { ["en"] = "At least 8 characters", ["fr"] = "Au moins 8 caractères" },
        ["Phone"] = new() { ["en"] = "Phone", ["fr"] = "Téléphone" },
        ["Date of birth"] = new() { ["en"] = "Date of birth", ["fr"] = "Date de naissance" },
        ["Gender"] = new() { ["en"] = "Gender", ["fr"] = "Genre" },
        ["Male"] = new() { ["en"] = "Male", ["fr"] = "Homme" },
        ["Female"] = new() { ["en"] = "Female", ["fr"] = "Femme" },
        ["Other"] = new() { ["en"] = "Other", ["fr"] = "Autre" },
        ["Already have an account?"] = new() { ["en"] = "Already have an account?", ["fr"] = "Vous avez déjà un compte ?" },
        ["Invalid email or password."] = new() { ["en"] = "Invalid email or password.", ["fr"] = "E-mail ou mot de passe invalide." },
        ["This account is inactive."] = new() { ["en"] = "This account is inactive.", ["fr"] = "Ce compte est inactif." },
        ["Log in failed. Please try again."] = new() { ["en"] = "Log in failed. Please try again.", ["fr"] = "Échec de la connexion. Veuillez réessayer." },
        ["An account with this email already exists."] = new()
        {
            ["en"] = "An account with this email already exists.",
            ["fr"] = "Un compte avec cette adresse e-mail existe déjà.",
        },
        ["Registration failed. Please check your details and try again."] = new()
        {
            ["en"] = "Registration failed. Please check your details and try again.",
            ["fr"] = "Échec de l'inscription. Veuillez vérifier vos informations et réessayer.",
        },

        // ── Patient: Login / Register ───────────────────────────────────
        ["Create your patient account"] = new() { ["en"] = "Create your patient account", ["fr"] = "Créez votre compte patient" },

        // ── Patient: Doctor profile ──────────────────────────────────────
        ["Doctor not found."] = new() { ["en"] = "Doctor not found.", ["fr"] = "Médecin introuvable." },
        ["perConsultation"] = new() { ["en"] = "${0} per consultation", ["fr"] = "{0} $ par consultation" },
        ["Book appointment"] = new() { ["en"] = "Book appointment", ["fr"] = "Prendre rendez-vous" },
        ["Availability"] = new() { ["en"] = "Availability", ["fr"] = "Disponibilités" },
        ["Check a date"] = new() { ["en"] = "Check a date", ["fr"] = "Vérifier une date" },
        ["No open slots on this date."] = new() { ["en"] = "No open slots on this date.", ["fr"] = "Aucun créneau disponible à cette date." },
        ["availableSlotsFor"] = new()
        {
            ["en"] = "For {0} you have the following available slots:",
            ["fr"] = "Pour le {0}, voici les créneaux disponibles :",
        },
        ["Reviews"] = new() { ["en"] = "Reviews", ["fr"] = "Avis" },
        ["No reviews yet."] = new() { ["en"] = "No reviews yet.", ["fr"] = "Aucun avis pour le moment." },

        // ── Patient: Book appointment ────────────────────────────────────
        ["Book an appointment"] = new() { ["en"] = "Book an appointment", ["fr"] = "Prendre un rendez-vous" },
        ["withDoctorSummary"] = new() { ["en"] = "with {0} — {1} (${2})", ["fr"] = "avec {0} — {1} ({2} $)" },
        ["Date"] = new() { ["en"] = "Date", ["fr"] = "Date" },
        ["selectedSlotFor"] = new() { ["en"] = "Selected slot for {0}:", ["fr"] = "Créneau sélectionné pour le {0} :" },
        ["Change"] = new() { ["en"] = "Change", ["fr"] = "Modifier" },
        ["Reason for visit (optional)"] = new() { ["en"] = "Reason for visit (optional)", ["fr"] = "Motif de la visite (facultatif)" },
        ["Confirm booking"] = new() { ["en"] = "Confirm booking", ["fr"] = "Confirmer le rendez-vous" },
        ["That slot was just taken. Please pick another time."] = new()
        {
            ["en"] = "That slot was just taken. Please pick another time.",
            ["fr"] = "Ce créneau vient d'être pris. Veuillez choisir un autre horaire.",
        },
        ["Booking failed. Please try again."] = new() { ["en"] = "Booking failed. Please try again.", ["fr"] = "Échec de la réservation. Veuillez réessayer." },

        // ── Statuses (Appointment + Payment share Pending/Completed) ────
        ["status.Pending"] = new() { ["en"] = "Pending", ["fr"] = "En attente" },
        ["status.Confirmed"] = new() { ["en"] = "Confirmed", ["fr"] = "Confirmé" },
        ["status.Completed"] = new() { ["en"] = "Completed", ["fr"] = "Terminé" },
        ["status.Cancelled"] = new() { ["en"] = "Cancelled", ["fr"] = "Annulé" },
        ["status.Rescheduled"] = new() { ["en"] = "Rescheduled", ["fr"] = "Reprogrammé" },
        ["status.Refunded"] = new() { ["en"] = "Refunded", ["fr"] = "Remboursé" },
        ["status.Failed"] = new() { ["en"] = "Failed", ["fr"] = "Échoué" },
        ["status.Approved"] = new() { ["en"] = "Approved", ["fr"] = "Approuvé" },
        ["status.Suspended"] = new() { ["en"] = "Suspended", ["fr"] = "Suspendu" },
        ["status.Rejected"] = new() { ["en"] = "Rejected", ["fr"] = "Rejeté" },

        // ── Patient: My appointments ─────────────────────────────────────
        ["My appointments"] = new() { ["en"] = "My appointments", ["fr"] = "Mes rendez-vous" },
        ["You have no appointments yet."] = new() { ["en"] = "You have no appointments yet.", ["fr"] = "Vous n'avez pas encore de rendez-vous." },
        ["to book one."] = new() { ["en"] = "to book one.", ["fr"] = "pour en réserver un." },
        ["Doctor"] = new() { ["en"] = "Doctor", ["fr"] = "Médecin" },
        ["Motif"] = new() { ["en"] = "Motif", ["fr"] = "Motif" },
        ["Status"] = new() { ["en"] = "Status", ["fr"] = "Statut" },
        ["View"] = new() { ["en"] = "View", ["fr"] = "Voir" },
        ["Unknown doctor"] = new() { ["en"] = "Unknown doctor", ["fr"] = "Médecin inconnu" },

        // ── Patient: Appointment detail ──────────────────────────────────
        ["Appointment not found."] = new() { ["en"] = "Appointment not found.", ["fr"] = "Rendez-vous introuvable." },
        ["appointmentWithDoctor"] = new() { ["en"] = "Appointment with {0}", ["fr"] = "Rendez-vous avec {0}" },
        ["refNumber"] = new() { ["en"] = "Ref #{0}", ["fr"] = "Réf n° {0}" },
        ["reasonLabel"] = new() { ["en"] = "Reason: {0}", ["fr"] = "Motif : {0}" },
        ["Change appointment"] = new() { ["en"] = "Change appointment", ["fr"] = "Modifier le rendez-vous" },
        ["Reschedule"] = new() { ["en"] = "Reschedule", ["fr"] = "Reprogrammer" },
        ["New date"] = new() { ["en"] = "New date", ["fr"] = "Nouvelle date" },
        ["newSlotFor"] = new() { ["en"] = "New slot for {0}:", ["fr"] = "Nouveau créneau pour le {0} :" },
        ["Confirm new time"] = new() { ["en"] = "Confirm new time", ["fr"] = "Confirmer le nouvel horaire" },
        ["Cancel"] = new() { ["en"] = "Cancel", ["fr"] = "Annuler" },
        ["That slot was just taken, or this appointment can no longer be rescheduled."] = new()
        {
            ["en"] = "That slot was just taken, or this appointment can no longer be rescheduled.",
            ["fr"] = "Ce créneau vient d'être pris, ou ce rendez-vous ne peut plus être reprogrammé.",
        },
        ["Could not reschedule. Please try again."] = new() { ["en"] = "Could not reschedule. Please try again.", ["fr"] = "Impossible de reprogrammer. Veuillez réessayer." },
        ["Payment"] = new() { ["en"] = "Payment", ["fr"] = "Paiement" },
        ["totalAmountLabel"] = new() { ["en"] = "Total: ${0}", ["fr"] = "Total : {0} $" },
        ["Pay now"] = new() { ["en"] = "Pay now", ["fr"] = "Payer maintenant" },
        ["Could not start payment. Please try again shortly."] = new()
        {
            ["en"] = "Could not start payment. Please try again shortly.",
            ["fr"] = "Impossible de démarrer le paiement. Veuillez réessayer sous peu.",
        },
        ["Card"] = new() { ["en"] = "Card", ["fr"] = "Carte" },
        ["Mobile Money"] = new() { ["en"] = "Mobile Money", ["fr"] = "Mobile Money" },
        ["Full name"] = new() { ["en"] = "Full name", ["fr"] = "Nom complet" },
        ["Phone number"] = new() { ["en"] = "Phone number", ["fr"] = "Numéro de téléphone" },
        ["Mobile money operator"] = new() { ["en"] = "Mobile money operator", ["fr"] = "Opérateur mobile money" },
        ["Check your phone to approve the payment."] = new()
        {
            ["en"] = "Check your phone to approve the payment.",
            ["fr"] = "Vérifiez votre téléphone pour approuver le paiement.",
        },
        ["Could not start the mobile money payment. Please try again."] = new()
        {
            ["en"] = "Could not start the mobile money payment. Please try again.",
            ["fr"] = "Impossible de démarrer le paiement mobile money. Veuillez réessayer.",
        },
        ["Leave a review"] = new() { ["en"] = "Leave a review", ["fr"] = "Laisser un avis" },
        ["No documents linked to this appointment."] = new()
        {
            ["en"] = "No documents linked to this appointment.",
            ["fr"] = "Aucun document associé à ce rendez-vous.",
        },
        ["Download"] = new() { ["en"] = "Download", ["fr"] = "Télécharger" },

        // ── Patient: Documents ───────────────────────────────────────────
        ["My documents"] = new() { ["en"] = "My documents", ["fr"] = "Mes documents" },
        ["Upload a document"] = new() { ["en"] = "Upload a document", ["fr"] = "Téléverser un document" },
        ["Category"] = new() { ["en"] = "Category", ["fr"] = "Catégorie" },
        ["docCategory.LabResult"] = new() { ["en"] = "Lab result", ["fr"] = "Résultat d'analyse" },
        ["docCategory.Prescription"] = new() { ["en"] = "Prescription", ["fr"] = "Ordonnance" },
        ["docCategory.Report"] = new() { ["en"] = "Report", ["fr"] = "Rapport" },
        ["docCategory.Other"] = new() { ["en"] = "Other", ["fr"] = "Autre" },
        ["Choose file"] = new() { ["en"] = "Choose file", ["fr"] = "Choisir un fichier" },
        ["Upload"] = new() { ["en"] = "Upload", ["fr"] = "Téléverser" },
        ["No documents uploaded yet."] = new() { ["en"] = "No documents uploaded yet.", ["fr"] = "Aucun document téléversé pour le moment." },
        ["File"] = new() { ["en"] = "File", ["fr"] = "Fichier" },
        ["Uploaded"] = new() { ["en"] = "Uploaded", ["fr"] = "Téléversé" },
        ["Upload failed. Please try again."] = new() { ["en"] = "Upload failed. Please try again.", ["fr"] = "Échec du téléversement. Veuillez réessayer." },
        ["File is too large (max 25 MB)."] = new() { ["en"] = "File is too large (max 25 MB).", ["fr"] = "Le fichier est trop volumineux (25 Mo maximum)." },

        // ── Patient: Notifications ────────────────────────────────────────
        ["No notifications."] = new() { ["en"] = "No notifications.", ["fr"] = "Aucune notification." },

        // ── Patient: Payment result ──────────────────────────────────────
        ["Payment received — thank you!"] = new() { ["en"] = "Payment received — thank you!", ["fr"] = "Paiement reçu — merci !" },
        ["Payment was cancelled. You can try again from your appointment."] = new()
        {
            ["en"] = "Payment was cancelled. You can try again from your appointment.",
            ["fr"] = "Le paiement a été annulé. Vous pouvez réessayer depuis votre rendez-vous.",
        },
        ["Confirming your payment…"] = new() { ["en"] = "Confirming your payment…", ["fr"] = "Confirmation de votre paiement en cours…" },
        ["We haven't received confirmation yet. This can take a moment — check back on your appointment shortly."] = new()
        {
            ["en"] = "We haven't received confirmation yet. This can take a moment — check back on your appointment shortly.",
            ["fr"] = "Nous n'avons pas encore reçu de confirmation. Cela peut prendre un moment — revenez bientôt sur votre rendez-vous.",
        },
        ["Back to appointment"] = new() { ["en"] = "Back to appointment", ["fr"] = "Retour au rendez-vous" },

        // ── Patient: Submit review ────────────────────────────────────────
        ["Rating"] = new() { ["en"] = "Rating", ["fr"] = "Note" },
        ["Comment (optional)"] = new() { ["en"] = "Comment (optional)", ["fr"] = "Commentaire (facultatif)" },
        ["Submit review"] = new() { ["en"] = "Submit review", ["fr"] = "Envoyer l'avis" },
        ["This appointment has already been reviewed."] = new() { ["en"] = "This appointment has already been reviewed.", ["fr"] = "Ce rendez-vous a déjà fait l'objet d'un avis." },
        ["Could not submit your review. Please try again."] = new()
        {
            ["en"] = "Could not submit your review. Please try again.",
            ["fr"] = "Impossible d'envoyer votre avis. Veuillez réessayer.",
        },

        // ── Doctor: Login / Register / Home ──────────────────────────────
        ["Create your doctor account"] = new() { ["en"] = "Create your doctor account", ["fr"] = "Créez votre compte médecin" },
        ["Manage your schedule, appointments, and patients in one place."] = new()
        {
            ["en"] = "Manage your schedule, appointments, and patients in one place.",
            ["fr"] = "Gérez votre horaire, vos rendez-vous et vos patients au même endroit.",
        },
        ["Go to appointments"] = new() { ["en"] = "Go to appointments", ["fr"] = "Voir mes rendez-vous" },
        ["Get started"] = new() { ["en"] = "Get started", ["fr"] = "Commencer" },

        // ── Doctor: Account status ────────────────────────────────────────
        ["Account status"] = new() { ["en"] = "Account status", ["fr"] = "Statut du compte" },
        ["You haven't set up your doctor profile yet."] = new()
        {
            ["en"] = "You haven't set up your doctor profile yet.",
            ["fr"] = "Vous n'avez pas encore configuré votre profil de médecin.",
        },
        ["Set up profile"] = new() { ["en"] = "Set up profile", ["fr"] = "Configurer le profil" },
        ["Edit profile"] = new() { ["en"] = "Edit profile", ["fr"] = "Modifier le profil" },
        ["Your account is approved. You're ready to see patients."] = new()
        {
            ["en"] = "Your account is approved. You're ready to see patients.",
            ["fr"] = "Votre compte est approuvé. Vous êtes prêt à recevoir des patients.",
        },
        ["Your account has been suspended. Contact support for details."] = new()
        {
            ["en"] = "Your account has been suspended. Contact support for details.",
            ["fr"] = "Votre compte a été suspendu. Contactez le support pour plus de détails.",
        },
        ["Your application was not approved."] = new() { ["en"] = "Your application was not approved.", ["fr"] = "Votre candidature n'a pas été approuvée." },
        ["Your profile is awaiting admin approval. You'll be able to accept appointments once approved."] = new()
        {
            ["en"] = "Your profile is awaiting admin approval. You'll be able to accept appointments once approved.",
            ["fr"] = "Votre profil est en attente d'approbation par un administrateur. Vous pourrez accepter des rendez-vous une fois approuvé.",
        },

        // ── Doctor: My profile ────────────────────────────────────────────
        ["My profile"] = new() { ["en"] = "My profile", ["fr"] = "Mon profil" },
        ["License number"] = new() { ["en"] = "License number", ["fr"] = "Numéro de licence" },
        ["Bio"] = new() { ["en"] = "Bio", ["fr"] = "Biographie" },
        ["Consultation fee ($)"] = new() { ["en"] = "Consultation fee ($)", ["fr"] = "Frais de consultation ($)" },
        ["Profile saved."] = new() { ["en"] = "Profile saved.", ["fr"] = "Profil enregistré." },
        ["Create profile"] = new() { ["en"] = "Create profile", ["fr"] = "Créer le profil" },
        ["Save changes"] = new() { ["en"] = "Save changes", ["fr"] = "Enregistrer les modifications" },
        ["Could not save your profile. Please try again."] = new()
        {
            ["en"] = "Could not save your profile. Please try again.",
            ["fr"] = "Impossible d'enregistrer votre profil. Veuillez réessayer.",
        },

        // ── Doctor: Schedule ──────────────────────────────────────────────
        ["Weekly schedule"] = new() { ["en"] = "Weekly schedule", ["fr"] = "Horaire hebdomadaire" },
        ["Day"] = new() { ["en"] = "Day", ["fr"] = "Jour" },
        ["Open"] = new() { ["en"] = "Open", ["fr"] = "Ouvert" },
        ["Open time"] = new() { ["en"] = "Open time", ["fr"] = "Heure d'ouverture" },
        ["Close time"] = new() { ["en"] = "Close time", ["fr"] = "Heure de fermeture" },
        ["Slot length (min)"] = new() { ["en"] = "Slot length (min)", ["fr"] = "Durée du créneau (min)" },
        ["day.Monday"] = new() { ["en"] = "Monday", ["fr"] = "Lundi" },
        ["day.Tuesday"] = new() { ["en"] = "Tuesday", ["fr"] = "Mardi" },
        ["day.Wednesday"] = new() { ["en"] = "Wednesday", ["fr"] = "Mercredi" },
        ["day.Thursday"] = new() { ["en"] = "Thursday", ["fr"] = "Jeudi" },
        ["day.Friday"] = new() { ["en"] = "Friday", ["fr"] = "Vendredi" },
        ["day.Saturday"] = new() { ["en"] = "Saturday", ["fr"] = "Samedi" },
        ["day.Sunday"] = new() { ["en"] = "Sunday", ["fr"] = "Dimanche" },
        ["Schedule saved."] = new() { ["en"] = "Schedule saved.", ["fr"] = "Horaire enregistré." },
        ["Save schedule"] = new() { ["en"] = "Save schedule", ["fr"] = "Enregistrer l'horaire" },
        ["Could not save your schedule. Please try again."] = new()
        {
            ["en"] = "Could not save your schedule. Please try again.",
            ["fr"] = "Impossible d'enregistrer votre horaire. Veuillez réessayer.",
        },

        // ── Doctor: Appointment detail ────────────────────────────────────
        ["appointmentOn"] = new() { ["en"] = "Appointment on {0}", ["fr"] = "Rendez-vous du {0}" },
        ["Mark as completed"] = new() { ["en"] = "Mark as completed", ["fr"] = "Marquer comme terminé" },
        ["Could not mark this appointment as completed."] = new()
        {
            ["en"] = "Could not mark this appointment as completed.",
            ["fr"] = "Impossible de marquer ce rendez-vous comme terminé.",
        },
        ["Patient documents"] = new() { ["en"] = "Patient documents", ["fr"] = "Documents du patient" },

        // ── Doctor: Reviews ────────────────────────────────────────────────
        ["My reviews"] = new() { ["en"] = "My reviews", ["fr"] = "Mes avis" },
        ["Average:"] = new() { ["en"] = "Average:", ["fr"] = "Moyenne :" },

        // ── Admin: Login / Overview ────────────────────────────────────────
        ["Admin log in"] = new() { ["en"] = "Admin log in", ["fr"] = "Connexion administrateur" },
        ["This account does not have admin access."] = new()
        {
            ["en"] = "This account does not have admin access.",
            ["fr"] = "Ce compte n'a pas d'accès administrateur.",
        },
        ["patientsAndDoctorsSummary"] = new() { ["en"] = "{0} patients · {1} doctors", ["fr"] = "{0} patients · {1} médecins" },

        // ── Admin: Doctors ────────────────────────────────────────────────
        ["Create doctor profile"] = new() { ["en"] = "Create doctor profile", ["fr"] = "Créer un profil médecin" },
        ["Doctor-role user"] = new() { ["en"] = "Doctor-role user", ["fr"] = "Utilisateur avec le rôle médecin" },
        ["Name"] = new() { ["en"] = "Name", ["fr"] = "Nom" },
        ["Actions"] = new() { ["en"] = "Actions", ["fr"] = "Actions" },
        ["Approve"] = new() { ["en"] = "Approve", ["fr"] = "Approuver" },
        ["Suspend"] = new() { ["en"] = "Suspend", ["fr"] = "Suspendre" },
        ["This user already has a doctor profile."] = new() { ["en"] = "This user already has a doctor profile.", ["fr"] = "Cet utilisateur a déjà un profil médecin." },
        ["Could not create the doctor profile."] = new() { ["en"] = "Could not create the doctor profile.", ["fr"] = "Impossible de créer le profil médecin." },
        ["All"] = new() { ["en"] = "All", ["fr"] = "Tous" },
        ["Filter by status"] = new() { ["en"] = "Filter by status", ["fr"] = "Filtrer par statut" },
        ["Create"] = new() { ["en"] = "Create", ["fr"] = "Créer" },

        // ── Admin: Clinics ────────────────────────────────────────────────
        ["Add clinic"] = new() { ["en"] = "Add clinic", ["fr"] = "Ajouter une clinique" },
        ["Type"] = new() { ["en"] = "Type", ["fr"] = "Type" },
        ["Address"] = new() { ["en"] = "Address", ["fr"] = "Adresse" },
        ["Could not create the clinic."] = new() { ["en"] = "Could not create the clinic.", ["fr"] = "Impossible de créer la clinique." },
        ["clinicType.PrivatePractice"] = new() { ["en"] = "Private Practice", ["fr"] = "Cabinet privé" },
        ["clinicType.Clinic"] = new() { ["en"] = "Clinic", ["fr"] = "Clinique" },
        ["clinicType.Hospital"] = new() { ["en"] = "Hospital", ["fr"] = "Hôpital" },
        ["clinicType.Lab"] = new() { ["en"] = "Lab", ["fr"] = "Laboratoire" },
    };
}
