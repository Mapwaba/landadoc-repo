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
        ["Search by doctor, specialty, hospital or city"] = new() { ["en"] = "Search by doctor, specialty, hospital or city", ["fr"] = "Rechercher par médecin, spécialité, hôpital ou ville" },
        ["searchResultCount"] = new() { ["en"] = "{0} result(s) for \"{1}\"", ["fr"] = "{0} résultat(s) pour « {1} »" },
        ["Statistics are unavailable right now."] = new() { ["en"] = "Statistics are unavailable right now.", ["fr"] = "Les statistiques sont indisponibles pour le moment." },
        ["Search is unavailable right now. Please try again in a minute."] = new() { ["en"] = "Search is unavailable right now. Please try again in a minute.", ["fr"] = "La recherche est indisponible pour le moment. Veuillez réessayer dans une minute." },
        ["Try again"] = new() { ["en"] = "Try again", ["fr"] = "Réessayer" },
        ["Dashboard"] = new() { ["en"] = "Dashboard", ["fr"] = "Tableau de bord" },
        ["Medical documents"] = new() { ["en"] = "Medical documents", ["fr"] = "Documents médicaux" },
        ["To view documents (prescriptions, tests, x-rays), open the relevant"] = new() { ["en"] = "To view documents (prescriptions, tests, x-rays), open the relevant", ["fr"] = "Pour consulter les documents (ordonnances, analyses, radios), veuillez accéder au" },
        ["Patient file"] = new() { ["en"] = "Patient file", ["fr"] = "Dossier patient" },
        ["Find a patient"] = new() { ["en"] = "Find a patient", ["fr"] = "Recherchez un patient" },
        ["Open the patient list to see their documents."] = new() { ["en"] = "Open the patient list to see their documents.", ["fr"] = "Accédez à la liste des patients pour voir leurs documents." },
        ["Go to patients"] = new() { ["en"] = "Go to patients", ["fr"] = "Aller aux patients" },
        ["Back to the list"] = new() { ["en"] = "Back to the list", ["fr"] = "Retour à la liste" },
        ["This patient isn't in your patient list."] = new() { ["en"] = "This patient isn't in your patient list.", ["fr"] = "Ce patient ne fait pas partie de votre liste de patients." },
        ["This patient file couldn't be loaded. Please try again in a minute."] = new() { ["en"] = "This patient file couldn't be loaded. Please try again in a minute.", ["fr"] = "Ce dossier patient n'a pas pu être chargé. Veuillez réessayer dans une minute." },
        ["Born on"] = new() { ["en"] = "Born on", ["fr"] = "Né(e) le" },
        ["Not provided"] = new() { ["en"] = "Not provided", ["fr"] = "Non renseigné" },
        ["Medical information"] = new() { ["en"] = "Medical information", ["fr"] = "Informations médicales" },
        ["Sex"] = new() { ["en"] = "Sex", ["fr"] = "Sexe" },
        ["Age"] = new() { ["en"] = "Age", ["fr"] = "Âge" },
        ["ageYears"] = new() { ["en"] = "{0} years", ["fr"] = "{0} ans" },
        ["Patient details couldn't be loaded. Please try again in a minute."] = new() { ["en"] = "Patient details couldn't be loaded. Please try again in a minute.", ["fr"] = "Les informations du patient n'ont pas pu être chargées. Veuillez réessayer dans une minute." },
        ["Medical history"] = new() { ["en"] = "Medical history", ["fr"] = "Historique médical" },
        ["New report"] = new() { ["en"] = "New report", ["fr"] = "Nouveau rapport" },
        ["Related appointment"] = new() { ["en"] = "Related appointment", ["fr"] = "Rendez-vous concerné" },
        ["None"] = new() { ["en"] = "None", ["fr"] = "Aucun" },
        ["No history available for this patient."] = new() { ["en"] = "No history available for this patient.", ["fr"] = "Aucun historique disponible pour ce patient." },
        ["Consultation"] = new() { ["en"] = "Consultation", ["fr"] = "Consultation" },
        ["The file must be smaller than 25 MB."] = new() { ["en"] = "The file must be smaller than 25 MB.", ["fr"] = "Le fichier doit faire moins de 25 Mo." },
        ["Report added to the patient file."] = new() { ["en"] = "Report added to the patient file.", ["fr"] = "Rapport ajouté au dossier du patient." },
        ["This document couldn't be opened. Please try again."] = new() { ["en"] = "This document couldn't be opened. Please try again.", ["fr"] = "Ce document n'a pas pu être ouvert. Veuillez réessayer." },
        ["My patients"] = new() { ["en"] = "My patients", ["fr"] = "Mes patients" },
        ["Search by last or first name..."] = new() { ["en"] = "Search by last or first name...", ["fr"] = "Rechercher par nom ou prénom..." },
        ["Sex / age"] = new() { ["en"] = "Sex / age", ["fr"] = "Genre / âge" },
        ["Payments & billing"] = new() { ["en"] = "Payments & billing", ["fr"] = "Paiements & facturation" },
        ["Total net revenue"] = new() { ["en"] = "Total net revenue", ["fr"] = "Revenu net total" },
        ["Transactions"] = new() { ["en"] = "Transactions", ["fr"] = "Transactions" },
        ["Transaction history"] = new() { ["en"] = "Transaction history", ["fr"] = "Historique des transactions" },
        ["Export"] = new() { ["en"] = "Export", ["fr"] = "Exporter" },
        ["Gross"] = new() { ["en"] = "Gross", ["fr"] = "Brut" },
        ["Net (received)"] = new() { ["en"] = "Net (received)", ["fr"] = "Net (reçu)" },
        ["appt"] = new() { ["en"] = "appt", ["fr"] = "RDV" },
        ["My public profile"] = new() { ["en"] = "My public profile", ["fr"] = "Mon profil public" },
        ["View my patient page"] = new() { ["en"] = "View my patient page", ["fr"] = "Voir ma fiche patient" },
        ["About"] = new() { ["en"] = "About", ["fr"] = "À propos" },
        ["Add a short presentation so patients get to know you."] = new() { ["en"] = "Add a short presentation so patients get to know you.", ["fr"] = "Ajoutez une courte présentation pour que les patients vous connaissent." },
        ["Contact details"] = new() { ["en"] = "Contact details", ["fr"] = "Coordonnées" },
        ["Professional information"] = new() { ["en"] = "Professional information", ["fr"] = "Informations pro" },
        ["Licence"] = new() { ["en"] = "Licence", ["fr"] = "Licence" },
        ["Manage my rates and acts"] = new() { ["en"] = "Manage my rates and acts", ["fr"] = "Gérer mes tarifs et actes" },
        ["Services & rates"] = new() { ["en"] = "Services & rates", ["fr"] = "Services & tarifs" },
        ["New service"] = new() { ["en"] = "New service", ["fr"] = "Nouveau service" },
        ["Medical act"] = new() { ["en"] = "Medical act", ["fr"] = "Acte médical" },
        ["Rate (USD)"] = new() { ["en"] = "Rate (USD)", ["fr"] = "Tarif (USD)" },
        ["Some documents couldn't be loaded. Please try again in a minute."] = new() { ["en"] = "Some documents couldn't be loaded. Please try again in a minute.", ["fr"] = "Certains documents n'ont pas pu être chargés. Veuillez réessayer dans une minute." },
        ["My workplace"] = new() { ["en"] = "My workplace", ["fr"] = "Mon établissement" },
        ["Your workplace couldn't be loaded. Please try again in a minute."] = new() { ["en"] = "Your workplace couldn't be loaded. Please try again in a minute.", ["fr"] = "Votre établissement n'a pas pu être chargé. Veuillez réessayer dans une minute." },
        ["You aren't linked to an establishment yet."] = new() { ["en"] = "You aren't linked to an establishment yet.", ["fr"] = "Vous n'êtes encore rattaché à aucun établissement." },
        ["Choose your clinics on your profile."] = new() { ["en"] = "Choose your clinics on your profile.", ["fr"] = "Choisissez vos cliniques dans votre profil." },
        ["Establishment logo"] = new() { ["en"] = "Establishment logo", ["fr"] = "Logo de la structure" },
        ["Choose a file"] = new() { ["en"] = "Choose a file", ["fr"] = "Choisir un fichier" },
        ["No file chosen"] = new() { ["en"] = "No file chosen", ["fr"] = "Aucun fichier choisi" },
        ["Remove"] = new() { ["en"] = "Remove", ["fr"] = "Retirer" },
        ["Establishment name"] = new() { ["en"] = "Establishment name", ["fr"] = "Nom de l'établissement" },
        ["Full address"] = new() { ["en"] = "Full address", ["fr"] = "Adresse complète" },
        ["Phone (main line)"] = new() { ["en"] = "Phone (main line)", ["fr"] = "Téléphone (standard)" },
        ["Email (contact)"] = new() { ["en"] = "Email (contact)", ["fr"] = "Email (contact)" },
        ["Enter a valid email address."] = new() { ["en"] = "Enter a valid email address.", ["fr"] = "Saisissez une adresse email valide." },
        ["Website"] = new() { ["en"] = "Website", ["fr"] = "Site web" },
        ["Description / presentation"] = new() { ["en"] = "Description / presentation", ["fr"] = "Description / présentation" },
        ["Please choose an image file."] = new() { ["en"] = "Please choose an image file.", ["fr"] = "Veuillez choisir une image." },
        ["The logo must be smaller than 500 KB."] = new() { ["en"] = "The logo must be smaller than 500 KB.", ["fr"] = "Le logo doit faire moins de 500 Ko." },
        ["This image couldn't be read. Please try another file."] = new() { ["en"] = "This image couldn't be read. Please try another file.", ["fr"] = "Cette image n'a pas pu être lue. Essayez un autre fichier." },
        ["Please fill in the highlighted fields."] = new() { ["en"] = "Please fill in the highlighted fields.", ["fr"] = "Veuillez remplir les champs en rouge." },
        ["Your workplace has been saved."] = new() { ["en"] = "Your workplace has been saved.", ["fr"] = "Votre établissement a été enregistré." },
        ["Could not save your workplace. Please check the fields and try again."] = new() { ["en"] = "Could not save your workplace. Please check the fields and try again.", ["fr"] = "Impossible d'enregistrer votre établissement. Vérifiez les champs et réessayez." },
        ["Search a patient"] = new() { ["en"] = "Search a patient", ["fr"] = "Rechercher un patient" },
        ["You don't have any patients yet."] = new() { ["en"] = "You don't have any patients yet.", ["fr"] = "Vous n'avez pas encore de patients." },
        ["No patient matches your search."] = new() { ["en"] = "No patient matches your search.", ["fr"] = "Aucun patient ne correspond à votre recherche." },
        ["Last visit"] = new() { ["en"] = "Last visit", ["fr"] = "Dernière visite" },
        ["Next appointment"] = new() { ["en"] = "Next appointment", ["fr"] = "Prochain rendez-vous" },
        ["Last reason"] = new() { ["en"] = "Last reason", ["fr"] = "Dernier motif" },
        ["Payments"] = new() { ["en"] = "Payments", ["fr"] = "Paiements" },
        ["Your payments couldn't be loaded. Please try again in a minute."] = new() { ["en"] = "Your payments couldn't be loaded. Please try again in a minute.", ["fr"] = "Vos paiements n'ont pas pu être chargés. Veuillez réessayer dans une minute." },
        ["Received (net)"] = new() { ["en"] = "Received (net)", ["fr"] = "Reçu (net)" },
        ["Awaiting payment"] = new() { ["en"] = "Awaiting payment", ["fr"] = "En attente de paiement" },
        ["Platform fees"] = new() { ["en"] = "Platform fees", ["fr"] = "Frais de plateforme" },
        ["No payments yet."] = new() { ["en"] = "No payments yet.", ["fr"] = "Aucun paiement pour le moment." },
        ["Method"] = new() { ["en"] = "Method", ["fr"] = "Moyen" },
        ["Amount"] = new() { ["en"] = "Amount", ["fr"] = "Montant" },
        ["Net"] = new() { ["en"] = "Net", ["fr"] = "Net" },
        ["All categories"] = new() { ["en"] = "All categories", ["fr"] = "Toutes les catégories" },
        ["Search a patient or file"] = new() { ["en"] = "Search a patient or file", ["fr"] = "Rechercher un patient ou un fichier" },
        ["Documents couldn't be loaded. Please try again in a minute."] = new() { ["en"] = "Documents couldn't be loaded. Please try again in a minute.", ["fr"] = "Les documents n'ont pas pu être chargés. Veuillez réessayer dans une minute." },
        ["Your patients haven't shared any documents yet."] = new() { ["en"] = "Your patients haven't shared any documents yet.", ["fr"] = "Vos patients n'ont encore partagé aucun document." },
        ["No document matches your search."] = new() { ["en"] = "No document matches your search.", ["fr"] = "Aucun document ne correspond à votre recherche." },
        ["Size"] = new() { ["en"] = "Size", ["fr"] = "Taille" },
        ["Services & acts"] = new() { ["en"] = "Services & acts", ["fr"] = "Services & actes" },
        ["Add a service"] = new() { ["en"] = "Add a service", ["fr"] = "Ajouter un service" },
        ["Service name"] = new() { ["en"] = "Service name", ["fr"] = "Nom du service" },
        ["e.g. Ultrasound"] = new() { ["en"] = "e.g. Ultrasound", ["fr"] = "ex. Échographie" },
        ["Price ($)"] = new() { ["en"] = "Price ($)", ["fr"] = "Prix ($)" },
        ["Duration (min)"] = new() { ["en"] = "Duration (min)", ["fr"] = "Durée (min)" },
        ["Description"] = new() { ["en"] = "Description", ["fr"] = "Description" },
        ["Your services couldn't be loaded. Please try again in a minute."] = new() { ["en"] = "Your services couldn't be loaded. Please try again in a minute.", ["fr"] = "Vos services n'ont pas pu être chargés. Veuillez réessayer dans une minute." },
        ["You haven't added any services yet."] = new() { ["en"] = "You haven't added any services yet.", ["fr"] = "Vous n'avez encore ajouté aucun service." },
        ["Service"] = new() { ["en"] = "Service", ["fr"] = "Service" },
        ["Duration"] = new() { ["en"] = "Duration", ["fr"] = "Durée" },
        ["Price"] = new() { ["en"] = "Price", ["fr"] = "Prix" },
        ["Edit"] = new() { ["en"] = "Edit", ["fr"] = "Modifier" },
        ["Delete"] = new() { ["en"] = "Delete", ["fr"] = "Supprimer" },
        ["Service saved."] = new() { ["en"] = "Service saved.", ["fr"] = "Service enregistré." },
        ["Could not save this service. Please try again."] = new() { ["en"] = "Could not save this service. Please try again.", ["fr"] = "Impossible d'enregistrer ce service. Veuillez réessayer." },
        ["Delete this service?"] = new() { ["en"] = "Delete this service?", ["fr"] = "Supprimer ce service ?" },
        ["Could not delete this service. Please try again."] = new() { ["en"] = "Could not delete this service. Please try again.", ["fr"] = "Impossible de supprimer ce service. Veuillez réessayer." },
        ["provider.Stripe"] = new() { ["en"] = "Card (Stripe)", ["fr"] = "Carte (Stripe)" },
        ["provider.MokoAfrika"] = new() { ["en"] = "Mobile money", ["fr"] = "Mobile money" },
        ["paymentStatus.Pending"] = new() { ["en"] = "Pending", ["fr"] = "En attente" },
        ["paymentStatus.Completed"] = new() { ["en"] = "Paid", ["fr"] = "Payé" },
        ["paymentStatus.Refunded"] = new() { ["en"] = "Refunded", ["fr"] = "Remboursé" },
        ["paymentStatus.Failed"] = new() { ["en"] = "Failed", ["fr"] = "Échoué" },
        ["My weekly template"] = new() { ["en"] = "My weekly template", ["fr"] = "Ma semaine type" },
        ["Save"] = new() { ["en"] = "Save", ["fr"] = "Enregistrer" },
        ["Appointment length"] = new() { ["en"] = "Appointment length", ["fr"] = "Durée des rendez-vous" },
        ["to"] = new() { ["en"] = "to", ["fr"] = "à" },
        ["Close this day"] = new() { ["en"] = "Close this day", ["fr"] = "Fermer ce jour" },
        ["Must end after it starts"] = new() { ["en"] = "Must end after it starts", ["fr"] = "L'heure de fin doit suivre l'heure de début" },
        ["Closed"] = new() { ["en"] = "Closed", ["fr"] = "Fermé" },
        ["Set your working hours"] = new() { ["en"] = "Set your working hours", ["fr"] = "Définir votre horaire de travail" },
        ["Please fix the highlighted hours before saving."] = new() { ["en"] = "Please fix the highlighted hours before saving.", ["fr"] = "Veuillez corriger les horaires en rouge avant d'enregistrer." },
        ["Your schedule couldn't be loaded. Please try again in a minute."] = new() { ["en"] = "Your schedule couldn't be loaded. Please try again in a minute.", ["fr"] = "Votre horaire n'a pas pu être chargé. Veuillez réessayer dans une minute." },
        ["Appointments today"] = new() { ["en"] = "Appointments today", ["fr"] = "Rendez-vous aujourd'hui" },
        ["Total patients"] = new() { ["en"] = "Total patients", ["fr"] = "Patients totaux" },
        ["Revenue (estimated)"] = new() { ["en"] = "Revenue (estimated)", ["fr"] = "Revenus (estimés)" },
        ["Next appointments"] = new() { ["en"] = "Next appointments", ["fr"] = "Prochains rendez-vous" },
        ["View agenda"] = new() { ["en"] = "View agenda", ["fr"] = "Voir l'agenda" },
        ["No upcoming appointments."] = new() { ["en"] = "No upcoming appointments.", ["fr"] = "Aucun rendez-vous à venir." },
        ["Today"] = new() { ["en"] = "Today", ["fr"] = "Aujourd'hui" },
        ["Patient"] = new() { ["en"] = "Patient", ["fr"] = "Patient" },
        ["Month"] = new() { ["en"] = "Month", ["fr"] = "Mois" },
        ["List"] = new() { ["en"] = "List", ["fr"] = "Liste" },
        ["Slot management"] = new() { ["en"] = "Slot management", ["fr"] = "Gestion des créneaux" },
        ["Weekly template"] = new() { ["en"] = "Weekly template", ["fr"] = "Semaine type" },
        ["Open for booking"] = new() { ["en"] = "Open for booking", ["fr"] = "Ouverts à la réservation" },
        ["Blocked slots"] = new() { ["en"] = "Blocked slots", ["fr"] = "Créneaux bloqués" },
        ["No appointments this day."] = new() { ["en"] = "No appointments this day.", ["fr"] = "Aucun rendez-vous ce jour." },
        ["No open slots this day."] = new() { ["en"] = "No open slots this day.", ["fr"] = "Aucun créneau ouvert ce jour." },
        ["Slots are unavailable right now."] = new() { ["en"] = "Slots are unavailable right now.", ["fr"] = "Les créneaux sont indisponibles pour le moment." },
        ["Your appointments couldn't be loaded. Please try again in a minute."] = new() { ["en"] = "Your appointments couldn't be loaded. Please try again in a minute.", ["fr"] = "Vos rendez-vous n'ont pas pu être chargés. Veuillez réessayer dans une minute." },
        ["Your account"] = new() { ["en"] = "Your account", ["fr"] = "Votre compte" },
        ["Your practice"] = new() { ["en"] = "Your practice", ["fr"] = "Votre activité" },
        ["You can add your clinics later on your profile."] = new() { ["en"] = "You can add your clinics later on your profile.", ["fr"] = "Vous pourrez ajouter vos cliniques plus tard dans votre profil." },
        ["Your account was created, but your profile couldn't be saved. Please try again."] = new() { ["en"] = "Your account was created, but your profile couldn't be saved. Please try again.", ["fr"] = "Votre compte a été créé, mais votre profil n'a pas pu être enregistré. Veuillez réessayer." },
        ["Family"] = new() { ["en"] = "Family", ["fr"] = "Famille" },
        ["Doctor space"] = new() { ["en"] = "Doctor", ["fr"] = "Médecin" },
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

        // ── Specialties (Specialties.cs) ────────────────────────────────
        ["Cardiologist"] = new() { ["en"] = "Cardiologist", ["fr"] = "Cardiologue" },
        ["Neurologist"] = new() { ["en"] = "Neurologist", ["fr"] = "Neurologue" },
        ["Generalist"] = new() { ["en"] = "Generalist", ["fr"] = "Généraliste" },
        ["Ophtamologist"] = new() { ["en"] = "Ophtamologist", ["fr"] = "Ophtalmologue" },
        ["Pediatrician"] = new() { ["en"] = "Pediatrician", ["fr"] = "Pédiatre" },
    };
}
