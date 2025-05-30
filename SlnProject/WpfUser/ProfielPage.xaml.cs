// ProfielPage.cs
// ---------------
// Dit is de profielpagina voor een gebruiker in de WpfUser-applicatie. 
// Hier kan de gebruiker zijn bedrijfsgegevens bekijken, aanpassen, en een wijzigingsaanvraag indienen. 
// Ook is het mogelijk om het bedrijfslogo te wijzigen.

using System.Windows;
using System.Windows.Controls;
using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Data;
using System.IO;
using Microsoft.Win32;
using System.Windows.Media.Imaging;

namespace WpfUser
{
    /// <summary>
    /// Pagina waarop de gebruiker zijn profiel kan aanpassen.
    /// </summary>
    public partial class ProfielPage : Page
    {
        private Frame frame;           // Referentie naar hoofdframe voor navigatie
        private Bedrijf bedrijf;       // Het huidige bedrijf (ingelogde gebruiker)
        private byte[] logoBytes;      // Opslag van het (nieuwe) logo als byte-array

        /// <summary>
        /// Constructor. Laadt alle bestaande bedrijfsgegevens in de velden.
        /// </summary>
        public ProfielPage(Frame frame)
        {
            InitializeComponent();
            this.frame = frame;
            int bedrijfId = (int)Application.Current.Properties["BedrijfId"];
            bedrijf = BedrijfData.GetById(bedrijfId);

            // Zet alle tekstvelden en logo
            txtNaam.Text = bedrijf.Naam;
            txtContact.Text = bedrijf.Contactpersoon;
            txtAdres.Text = bedrijf.Adres;
            txtPostcode.Text = bedrijf.Postcode;
            txtGemeente.Text = bedrijf.Gemeente;
            txtLand.Text = bedrijf.Land;
            txtTelefoon.Text = bedrijf.Telefoon;
            txtEmail.Text = bedrijf.Email;
            txtBtwNummer.Text = bedrijf.BtwNummer;
            txtLogin.Text = bedrijf.Login;
            txtTaal.Text = bedrijf.Taal;
            txtNacecode.Text = bedrijf.Nacecode;

            logoBytes = bedrijf.Logo; // Startwaarde is huidige logo
            if (logoBytes != null && logoBytes.Length > 0)
                imgLogo.Source = ByteArrayToImage(logoBytes);
        }

        /// <summary>
        /// Zet een byte-array (afbeelding) om naar een BitmapImage voor WPF.
        /// </summary>
        private BitmapImage ByteArrayToImage(byte[] bytes)
        {
            BitmapImage bitmap = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
            }
            return bitmap;
        }

        /// <summary>
        /// Selecteer een nieuw logobestand en toon het in het profielscherm.
        /// </summary>
        private void BtnSelectLogo_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Afbeeldingen (*.png;*.jpg)|*.png;*.jpg";
            if (dlg.ShowDialog() == true)
            {
                logoBytes = File.ReadAllBytes(dlg.FileName);
                imgLogo.Source = ByteArrayToImage(logoBytes);
            }
        }

        /// <summary>
        /// Slaat ALLE gewijzigde gegevens als een nieuwe wijzigingsaanvraag op in JSON.
        /// </summary>
        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            // Neem alle huidige velden over in de aanvraag
            RegistratieAanvraag aanvraag = new RegistratieAanvraag
            {
                BedrijfId = bedrijf.Id,
                Naam = txtNaam.Text,
                Contactpersoon = txtContact.Text,
                Adres = txtAdres.Text,
                Postcode = txtPostcode.Text,
                Gemeente = txtGemeente.Text,
                Land = txtLand.Text,
                Telefoon = txtTelefoon.Text,
                Email = txtEmail.Text,
                BtwNummer = txtBtwNummer.Text,
                Login = txtLogin.Text,
                Wachtwoord = string.IsNullOrWhiteSpace(pwdWachtwoord.Password) ? null : pwdWachtwoord.Password, // Alleen indien ingevuld
                Taal = txtTaal.Text,
                Nacecode = txtNacecode.Text,
                Logo = logoBytes,
                Registratiedatum = System.DateTime.Now,
                Type = "wijziging",
                Status = "nieuw"
            };
            RegistratieAanvraagData.Insert(aanvraag);
            txtFeedback.Text = "Wijziging is ingediend en wacht op goedkeuring!";
        }

        /// <summary>
        /// Navigeert terug naar het dashboard (home menu).
        /// </summary>
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new DashboardPage(frame);
        }
    }
}
