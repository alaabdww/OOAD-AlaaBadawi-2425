// RegistratieAanvraag.cs
// ----------------------
// Dit model stelt een registratie-aanvraag voor, zoals ingediend vanuit de WpfUser-app.
// Kan zowel een aanvraag voor een nieuw bedrijf als een wijzigingsverzoek voor een bestaand bedrijf bevatten.
// Wordt o.a. gebruikt om profielwijzigingen tijdelijk op te slaan voordat de admin deze goedkeurt of weigert.

using System;

namespace BenchmarkToolLibrary.Models
{
    public class RegistratieAanvraag
    {
        public int Id { get; set; }                         // Uniek Id van de aanvraag
        public int? BedrijfId { get; set; }                 // BedrijfId als het een wijziging is, anders null voor nieuw bedrijf
        public string Naam { get; set; }                    // Bedrijfsnaam
        public string Contactpersoon { get; set; }          // Naam van de contactpersoon
        public string Adres { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }
        public string Land { get; set; }
        public string Telefoon { get; set; }
        public string Email { get; set; }
        public string BtwNummer { get; set; }
        public string Login { get; set; }
        public string Wachtwoord { get; set; }              // Enkel ingevuld bij nieuwe aanvraag of wachtwoordwijziging
        public DateTime Registratiedatum { get; set; }      // Wanneer werd de aanvraag ingediend?
        public string Taal { get; set; }                    // Voorkeurstaal
        public byte[] Logo { get; set; }                    // Optioneel: nieuw logo
        public string Nacecode { get; set; }                // Sectorcode
        public string Type { get; set; }                    // "nieuw" (nieuwe registratie) of "wijziging"
        public string Status { get; set; }                  // "nieuw", "goedgekeurd", "geweigerd"
    }
}
