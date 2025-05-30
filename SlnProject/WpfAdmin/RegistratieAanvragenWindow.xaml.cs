// RegistratieAanvragenWindow.cs
// -----------------------------
// Dit WPF-venster toont alle nieuwe registratieaanvragen (nieuwe bedrijven OF wijzigingsaanvragen).
// De admin kan details bekijken, aanvragen goedkeuren (=> wordt bedrijf in database), of weigeren (status geweigerd).

using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Data;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;

namespace WpfAdmin
{
    public partial class RegistratieAanvragenWindow : Window
    {
        // Lijst van alle huidige aanvragen met status "nieuw".
        private List<RegistratieAanvraag> aanvragen;

        /// <summary>
        /// Constructor. Laadt direct de aanvragen.
        /// </summary>
        public RegistratieAanvragenWindow()
        {
            InitializeComponent();
            LaadAanvragen();
        }

        /// <summary>
        /// Haalt alle 'nieuwe' aanvragen op en vult de lijst in de UI.
        /// </summary>
        private void LaadAanvragen()
        {
            aanvragen = RegistratieAanvraagData.GetAllNieuw();
            lstAanvragen.ItemsSource = aanvragen;
            lstAanvragen.SelectedIndex = -1;
            ToonDetails(null);
        }

        /// <summary>
        /// Wordt uitgevoerd als een andere aanvraag wordt aangeklikt in de lijst. Toont de details.
        /// </summary>
        private void lstAanvragen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RegistratieAanvraag aanvraag = lstAanvragen.SelectedItem as RegistratieAanvraag;
            ToonDetails(aanvraag);
        }

        /// <summary>
        /// Zet de details van de geselecteerde aanvraag in de UI.
        /// </summary>
        private void ToonDetails(RegistratieAanvraag aanvraag)
        {
            if (aanvraag != null)
            {
                lblNaam.Content = aanvraag.Naam;
                lblContactpersoon.Content = aanvraag.Contactpersoon;
                lblAdres.Content = aanvraag.Adres;
                lblPostcode.Content = aanvraag.Postcode;
                lblGemeente.Content = aanvraag.Gemeente;
                lblLogin.Content = aanvraag.Login;
                lblEmail.Content = aanvraag.Email;
                lblStatus.Content = aanvraag.Status;

                // Toon logo indien aanwezig
                if (aanvraag.Logo != null && aanvraag.Logo.Length > 0)
                {
                    BitmapImage bitmap = new BitmapImage();
                    using (MemoryStream ms = new MemoryStream(aanvraag.Logo))
                    {
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = ms;
                        bitmap.EndInit();
                    }
                    imgLogo.Source = bitmap;
                }
                else
                {
                    imgLogo.Source = null;
                }
            }
            else
            {
                // Clear de UI indien geen aanvraag geselecteerd
                lblNaam.Content = "";
                lblContactpersoon.Content = "";
                lblAdres.Content = "";
                lblPostcode.Content = "";
                lblGemeente.Content = "";
                lblLogin.Content = "";
                lblEmail.Content = "";
                lblStatus.Content = "";
                imgLogo.Source = null;
            }

            txtFeedback.Text = "";
        }

        /// <summary>
        /// Keurt een aanvraag goed:
        /// - Als het een nieuw bedrijf is: voegt toe aan Companies.
        /// - Als het een wijziging is: past bestaand bedrijf aan.
        /// - Verwijdert daarna de aanvraag.
        /// </summary>
        private void btnGoedkeuren_Click(object sender, RoutedEventArgs e)
        {
            RegistratieAanvraag aanvraag = lstAanvragen.SelectedItem as RegistratieAanvraag;
            if (aanvraag == null)
            {
                txtFeedback.Text = "Selecteer een aanvraag.";
                return;
            }

            if (aanvraag.BedrijfId == null || aanvraag.BedrijfId == 0)
            {
                // Nieuwe registratie: maak een Bedrijf aan op basis van de aanvraag en voeg toe aan Companies.
                Bedrijf bedrijf = new Bedrijf
                {
                    Naam = aanvraag.Naam,
                    Contactpersoon = aanvraag.Contactpersoon,
                    Adres = aanvraag.Adres,
                    Postcode = aanvraag.Postcode,
                    Gemeente = aanvraag.Gemeente,
                    Land = aanvraag.Land,
                    Telefoon = aanvraag.Telefoon,
                    Email = aanvraag.Email,
                    BtwNummer = aanvraag.BtwNummer,
                    Login = aanvraag.Login,
                    Wachtwoord = aanvraag.Wachtwoord,
                    Registratiedatum = aanvraag.Registratiedatum,
                    Acceptatiedatum = System.DateTime.Now,
                    LaatstGewijzigd = System.DateTime.Now,
                    Taal = aanvraag.Taal,
                    Logo = aanvraag.Logo,
                    Nacecode = aanvraag.Nacecode,
                    Status = "actief"
                };
                BedrijfData.Insert(bedrijf);
            }
            else
            {
                // Wijziging: haal bestaand bedrijf op en pas eigenschappen aan.
                Bedrijf bedrijf = BedrijfData.GetById(aanvraag.BedrijfId.Value);
                if (bedrijf != null)
                {
                    bedrijf.Naam = aanvraag.Naam;
                    bedrijf.Contactpersoon = aanvraag.Contactpersoon;
                    bedrijf.Adres = aanvraag.Adres;
                    bedrijf.Postcode = aanvraag.Postcode;
                    bedrijf.Gemeente = aanvraag.Gemeente;
                    bedrijf.Land = aanvraag.Land;
                    bedrijf.Telefoon = aanvraag.Telefoon;
                    bedrijf.Email = aanvraag.Email;
                    bedrijf.BtwNummer = aanvraag.BtwNummer;
                    bedrijf.Login = aanvraag.Login;
                    bedrijf.Taal = aanvraag.Taal;
                    bedrijf.Logo = aanvraag.Logo;
                    bedrijf.Nacecode = aanvraag.Nacecode;
                    bedrijf.Status = "actief";
                    bedrijf.LaatstGewijzigd = System.DateTime.Now;

                    // Indien wachtwoord is ingevuld, update het wachtwoord
                    if (!string.IsNullOrWhiteSpace(aanvraag.Wachtwoord))
                    {
                        BedrijfData.Update(bedrijf, aanvraag.Wachtwoord);
                    }
                    else
                    {
                        BedrijfData.Update(bedrijf, null);
                    }
                }
            }

            // Verwijder de aanvraag nadat ze behandeld is
            RegistratieAanvraagData.Delete(aanvraag.Id);
            LaadAanvragen();
        }

        /// <summary>
        /// Weigert een aanvraag: zet status op "geweigerd" en verwijdert uit de lijst.
        /// </summary>
        private void btnWeigeren_Click(object sender, RoutedEventArgs e)
        {
            RegistratieAanvraag aanvraag = lstAanvragen.SelectedItem as RegistratieAanvraag;
            if (aanvraag == null)
            {
                txtFeedback.Text = "Selecteer een aanvraag.";
                return;
            }
            RegistratieAanvraagData.UpdateStatus(aanvraag.Id, "geweigerd");
            RegistratieAanvraagData.Delete(aanvraag.Id);
            LaadAanvragen();
        }
    }
}
