// BedrijfNieuwWindow.cs
// ---------------------
// Dit WPF-window laat een admin toe een nieuwe bedrijfsaanvraag in te dienen. 
// De gegevens worden als RegistratieAanvraag opgeslagen (voor goedkeuring), niet direct in de database.

using BenchmarkToolLibrary.Data;
using BenchmarkToolLibrary.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfAdmin
{
    public partial class BedrijfNieuwWindow : Window
    {
        // Houdt de bytes van het gekozen logo bij.
        private byte[] logoBytes = null;

        /// <summary>
        /// Constructor voor het venster. Initialiseert de componenten.
        /// </summary>
        public BedrijfNieuwWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handler voor de "Selecteer logo..."-knop. Opent een dialoog om een afbeelding te kiezen, en toont deze.
        /// </summary>
        private void BtnSelectLogo_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Afbeeldingen (*.png;*.jpg)|*.png;*.jpg";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                logoBytes = File.ReadAllBytes(dlg.FileName);
                BitmapImage bitmap = new BitmapImage();
                MemoryStream ms = new MemoryStream(logoBytes);
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                imgLogo.Source = bitmap;
            }
        }

        /// <summary>
        /// Handler voor de "Opslaan"-knop. Valideert alle velden, maakt een aanvraag en voegt deze toe aan de lijst van aanvragen.
        /// </summary>
        private void BtnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            txtFeedback.Text = "";
            try
            {
                // Maak een nieuwe aanvraag op basis van de ingevulde velden
                RegistratieAanvraag aanvraag = new RegistratieAanvraag();
                aanvraag.Naam = txtNaam.Text.Trim();
                aanvraag.Contactpersoon = txtContactpersoon.Text.Trim();
                aanvraag.Adres = txtAdres.Text.Trim();
                aanvraag.Postcode = txtPostcode.Text.Trim();
                aanvraag.Gemeente = txtGemeente.Text.Trim();
                aanvraag.Land = txtLand.Text.Trim();
                aanvraag.Telefoon = txtTelefoon.Text.Trim();
                aanvraag.Email = txtEmail.Text.Trim();
                aanvraag.BtwNummer = txtBtwNummer.Text.Trim();
                aanvraag.Login = txtLogin.Text.Trim();
                aanvraag.Wachtwoord = pwdWachtwoord.Password;
                aanvraag.Registratiedatum = DateTime.Now;
                aanvraag.Taal = txtTaal.Text.Trim();
                aanvraag.Logo = logoBytes;
                aanvraag.Nacecode = txtNacecode.Text.Trim();
                aanvraag.Type = "nieuw";
                aanvraag.Status = "nieuw";

                // Controleer dat elk veld is ingevuld (inclusief logo en nacecode)
                if (string.IsNullOrWhiteSpace(aanvraag.Naam) ||
                    string.IsNullOrWhiteSpace(aanvraag.Contactpersoon) ||
                    string.IsNullOrWhiteSpace(aanvraag.Adres) ||
                    string.IsNullOrWhiteSpace(aanvraag.Postcode) ||
                    string.IsNullOrWhiteSpace(aanvraag.Gemeente) ||
                    string.IsNullOrWhiteSpace(aanvraag.Land) ||
                    string.IsNullOrWhiteSpace(aanvraag.Telefoon) ||
                    string.IsNullOrWhiteSpace(aanvraag.Email) ||
                    string.IsNullOrWhiteSpace(aanvraag.BtwNummer) ||
                    string.IsNullOrWhiteSpace(aanvraag.Login) ||
                    string.IsNullOrWhiteSpace(aanvraag.Wachtwoord) ||
                    string.IsNullOrWhiteSpace(aanvraag.Taal) ||
                    aanvraag.Logo == null || aanvraag.Logo.Length == 0 ||
                    string.IsNullOrWhiteSpace(aanvraag.Nacecode))
                {
                    txtFeedback.Text = "Vul ALLE velden in (inclusief logo en nacecode)!";
                    return;
                }

                // Sla de aanvraag op (voor goedkeuring door admin)
                RegistratieAanvraagData.Insert(aanvraag);

                txtFeedback.Foreground = System.Windows.Media.Brushes.Green;
                txtFeedback.Text = "Bedrijf-aanvraag is ingediend en wacht op goedkeuring!";
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                txtFeedback.Foreground = System.Windows.Media.Brushes.Red;
                txtFeedback.Text = "Fout: " + ex.Message;
            }
        }

        /// <summary>
        /// Handler voor de "Annuleren"-knop. Sluit het venster zonder iets op te slaan.
        /// </summary>
        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
