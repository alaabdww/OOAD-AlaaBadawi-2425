// BedrijfEditWindow.xaml.cs
// -------------------------
// Dit is het window waarmee een admin een bestaand bedrijf kan bewerken in WpfAdmin.
// De wijzigingen worden NIET direct in de database opgeslagen, maar als wijzigingsaanvraag toegevoegd
// zodat ze later door een beheerder goedgekeurd kunnen worden.

using BenchmarkToolLibrary.Data;
using BenchmarkToolLibrary.Models;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfAdmin
{
    public partial class BedrijfEditWindow : Window
    {
        private Bedrijf bedrijf;      // Huidig bewerkt bedrijf (object)
        private byte[] logoBytes;     // Logo-afbeelding in bytes

        /// <summary>
        /// Constructor: vult alle velden in met de gegevens van het bedrijf.
        /// </summary>
        public BedrijfEditWindow(Bedrijf bedrijf)
        {
            InitializeComponent();
            this.bedrijf = bedrijf;

            // Vul alle invoervelden met de bestaande bedrijfsdata
            txtNaam.Text = bedrijf.Naam;
            txtContactpersoon.Text = bedrijf.Contactpersoon;
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

            // Status combobox initialiseren
            foreach (object obj in cmbStatus.Items)
            {
                ComboBoxItem item = obj as ComboBoxItem;
                if (item != null && (string)item.Content == (bedrijf.Status ?? "nieuw"))
                {
                    cmbStatus.SelectedItem = item;
                    break;
                }
            }

            // Logo weergeven indien aanwezig
            if (bedrijf.Logo != null && bedrijf.Logo.Length > 0)
            {
                logoBytes = bedrijf.Logo;
                BitmapImage bitmap = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(logoBytes))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                }
                imgLogo.Source = bitmap;
            }
        }

        /// <summary>
        /// Selecteert een nieuw logo via een dialoogvenster en toont deze in de UI.
        /// </summary>
        private void BtnSelectLogo_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Afbeeldingen (*.png;*.jpg)|*.png;*.jpg";
            bool? result = dlg.ShowDialog();
            if (result.HasValue && result.Value)
            {
                logoBytes = File.ReadAllBytes(dlg.FileName);
                BitmapImage bitmap = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(logoBytes))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                }
                imgLogo.Source = bitmap;
            }
        }

        /// <summary>
        /// Valideert de ingevulde gegevens en slaat de wijzigingsaanvraag op in een JSON-lijst (niet direct in Companies).
        /// </summary>
        private void BtnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            txtFeedback.Text = "";
            try
            {
                RegistratieAanvraag aanvraag = new RegistratieAanvraag();
                aanvraag.BedrijfId = bedrijf.Id;
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
                aanvraag.Taal = txtTaal.Text.Trim();
                aanvraag.Nacecode = txtNacecode.Text.Trim();
                aanvraag.Logo = logoBytes;
                aanvraag.Registratiedatum = DateTime.Now;
                aanvraag.Type = "wijziging";
                aanvraag.Status = "nieuw";

                // Belangrijke validatie: deze velden zijn verplicht!
                if (string.IsNullOrWhiteSpace(aanvraag.Naam) ||
                    string.IsNullOrWhiteSpace(aanvraag.Login))
                {
                    txtFeedback.Text = "Naam en login mogen niet leeg zijn.";
                    return;
                }

                RegistratieAanvraagData.Insert(aanvraag);

                txtFeedback.Foreground = System.Windows.Media.Brushes.Green;
                txtFeedback.Text = "Wijzigingsaanvraag is ingediend en wacht op goedkeuring!";
                // Window blijft open voor feedback
            }
            catch (Exception ex)
            {
                txtFeedback.Foreground = System.Windows.Media.Brushes.Red;
                txtFeedback.Text = "Fout bij opslaan: " + ex.Message;
            }
        }

        /// <summary>
        /// Annuleert de bewerking en sluit het venster.
        /// </summary>
        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
