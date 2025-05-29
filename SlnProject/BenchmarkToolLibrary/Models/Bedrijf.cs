using System;
using System.Net.Mail;

namespace BenchmarkToolLibrary.Models
{
    public class Bedrijf
    {
        public int Id { get; private set; }

        public string Naam { get; set; }
        public string Contactpersoon { get; set; }
        public string Adres { get; set; }
        public string Postcode { get; set; }
        public string Gemeente { get; set; }
        public string Land { get; set; }
        public string Telefoon { get; set; }

        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();
                try
                {
                    MailAddress addr = new MailAddress(value);
                    _email = value;
                }
                catch
                {
                    throw new ArgumentException();
                }
            }
        }

        public string BtwNummer { get; set; }
        public string Login { get; set; }
        public string Wachtwoord { get; set; }
        public DateTime Registratiedatum { get; set; }
        public DateTime? Acceptatiedatum { get; set; }
        public DateTime LaatstGewijzigd { get; set; }

        private string _status = "nieuw";
        public string Status
        {
            get { return _status; }
            set { _status = value ?? "nieuw"; }
        }

        public string Taal { get; set; }
        public byte[] Logo { get; set; }
        public string Nacecode { get; set; }

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

        public Bedrijf() { }

        // Setter zodat Data-layer Id kan instellen na Insert
        public void SetId(int id)
        {
            Id = id;
        }

        // Clone voor edit-copie
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
