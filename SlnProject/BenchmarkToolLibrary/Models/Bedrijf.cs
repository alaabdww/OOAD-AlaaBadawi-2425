using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BenchmarkToolLibrary.Models
{
    public class Bedrijf
    {
        public int Id { get; }

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
            get => _email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException();
                try
                {
                    var addr = new MailAddress(value);
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
            get => _status;
            set => _status = value ?? "nieuw";
        }

        public string Taal { get; set; }
        public string Logo { get; set; }
        public string Nacecode { get; set; }

        public Bedrijf()
        {
            _status = "nieuw";
            Registratiedatum = DateTime.Now;
            LaatstGewijzigd = DateTime.Now;
        }

        public Bedrijf(string naam, string contactpersoon, string adres, string postcode, string gemeente,
            string land, string telefoon, string email, string btwNummer, string login, string wachtwoord,
            DateTime registratiedatum, DateTime? acceptatiedatum, DateTime laatstGewijzigd, string status,
            string taal, string logo, string nacecode)
        {
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
    }
}
