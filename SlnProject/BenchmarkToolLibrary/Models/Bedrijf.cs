// Bedrijf.cs
// ----------------------
// Dit model stelt een bedrijf (klant) voor in de BenchmarkTool.
// Het bevat alle bedrijfsgegevens zoals adres, contact, status, login, taal, logo en nacecode.
// Dit model wordt gebruikt voor opslag, bewerking en uitwisseling van bedrijfsinfo tussen database en UI.

using System;

namespace BenchmarkToolLibrary.Models
{
    public class Bedrijf
    {
        // Unieke identificatie van het bedrijf (meestal de primary key in de database)
        public int Id { get; private set; }

        // Bedrijfsnaam
        public string Naam { get; set; }
        // Naam van de contactpersoon bij het bedrijf
        public string Contactpersoon { get; set; }
        // Straat + huisnummer
        public string Adres { get; set; }
        // Postcode
        public string Postcode { get; set; }
        // Gemeente/stad
        public string Gemeente { get; set; }
        // Land
        public string Land { get; set; }
        // Telefoonnummer
        public string Telefoon { get; set; }

        // Emailadres van het bedrijf (mag leeg zijn)
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                // Email mag null of leeg zijn!
                _email = value;
            }
        }

        // BTW-nummer (mag leeg zijn)
        public string BtwNummer { get; set; }
        // Loginnaam voor authenticatie
        public string Login { get; set; }
        // Wachtwoord (nooit uitlezen, alleen voor opslag/hash)
        public string Wachtwoord { get; set; }
        // Datum van registratie
        public DateTime Registratiedatum { get; set; }
        // Optionele datum waarop het bedrijf geaccepteerd werd
        public DateTime? Acceptatiedatum { get; set; }
        // Laatste wijzigingsdatum van het bedrijf
        public DateTime LaatstGewijzigd { get; set; }

        // Status van het bedrijf ("nieuw", "actief", ...)
        private string _status = "nieuw";
        public string Status
        {
            get { return _status; }
            set { _status = value ?? "nieuw"; }
        }

        // Voorkeurstaal (bijv. "NL", "FR", ...)
        public string Taal { get; set; }
        // Bedrijfslogo als byte-array (kan null zijn)
        public byte[] Logo { get; set; }
        // Nacecode van het bedrijf (branche-indicatie)
        public string Nacecode { get; set; }

        /// <summary>
        /// Constructor voor volledig ingevuld bedrijf (meestal gebruikt door database-laag)
        /// </summary>
        public Bedrijf(int id, string naam, string contactpersoon, string adres, string postcode, string gemeente,
            string land, string telefoon, string email, string btwNummer, string login, string wachtwoord,
            DateTime registratiedatum, DateTime? acceptatiedatum, DateTime laatstGewijzigd, string status,
            string taal, byte[] logo, string nacecode)
        {
            Id = id;
            Naam = naam;
            Contactpersoon = contactpersoon;
            Adres = adres;
            Postcode = postcode;
            Gemeente = gemeente;
            Land = land;
            Telefoon = telefoon;
            Email = email;
            BtwNummer = btwNummer;
            Login = login;
            Wachtwoord = wachtwoord;
            Registratiedatum = registratiedatum;
            Acceptatiedatum = acceptatiedatum;
            LaatstGewijzigd = laatstGewijzigd;
            Status = status ?? "nieuw";
            Taal = taal;
            Logo = logo;
            Nacecode = nacecode;
        }

        /// <summary>
        /// Parameterloze constructor (nodig voor JSON of handmatige creatie)
        /// </summary>
        public Bedrijf() { }

        /// <summary>
        /// Stelt het ID van het bedrijf in (na toevoegen aan database)
        /// </summary>
        public void SetId(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Maak een volledige kopie van het bedrijf (voor edit zonder originele te wijzigen)
        /// </summary>
        public Bedrijf Clone()
        {
            return new Bedrijf(
                Id, Naam, Contactpersoon, Adres, Postcode, Gemeente, Land, Telefoon, Email, BtwNummer, Login,
                Wachtwoord, Registratiedatum, Acceptatiedatum, LaatstGewijzigd, Status, Taal,
                Logo != null ? (byte[])Logo.Clone() : null, Nacecode
            );
        }
    }
}
