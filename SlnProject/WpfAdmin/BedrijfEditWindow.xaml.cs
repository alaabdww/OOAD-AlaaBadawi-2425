using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Data;

namespace WpfAdmin
{
    public partial class BedrijfEditWindow : Window
    {
        private Bedrijf bedrijf;
        private byte[] logoBytes = null;

        public BedrijfEditWindow(Bedrijf bestaandBedrijf)
        {
            InitializeComponent();
            bedrijf = bestaandBedrijf;
            ToonBedrijf();
        }

        private void ToonBedrijf()
        {
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
            logoBytes = bedrijf.Logo;

            if (logoBytes != null && logoBytes.Length > 0)
            {
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
            else
            {
                imgLogo.Source = null;
            }

            cmbStatus.Items.Clear();
            cmbStatus.Items.Add("nieuw");
            cmbStatus.Items.Add("actief");
            cmbStatus.Items.Add("geweigerd");
            cmbStatus.SelectedItem = bedrijf.Status;
        }

        private void BtnSelectLogo_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Afbeeldingen (*.png;*.jpg)|*.png;*.jpg";
            if (dlg.ShowDialog() == true)
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

        private void BtnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            txtFeedback.Text = "";
            try
            {
                bedrijf.Naam = txtNaam.Text.Trim();
                bedrijf.Contactpersoon = txtContactpersoon.Text.Trim();
                bedrijf.Adres = txtAdres.Text.Trim();
                bedrijf.Postcode = txtPostcode.Text.Trim();
                bedrijf.Gemeente = txtGemeente.Text.Trim();
                bedrijf.Land = txtLand.Text.Trim();
                bedrijf.Telefoon = txtTelefoon.Text.Trim();
                bedrijf.Email = txtEmail.Text.Trim();
                bedrijf.BtwNummer = txtBtwNummer.Text.Trim();
                bedrijf.Login = txtLogin.Text.Trim();
                bedrijf.Taal = txtTaal.Text.Trim();
                bedrijf.Nacecode = txtNacecode.Text.Trim();
                bedrijf.Logo = logoBytes;
                bedrijf.Status = (string)cmbStatus.SelectedItem;
                bedrijf.LaatstGewijzigd = DateTime.Now;

                string nieuwWachtwoord = pwdNieuwWachtwoord.Password;
                if (!string.IsNullOrWhiteSpace(nieuwWachtwoord))
                {
                    BedrijfData.Update(bedrijf, nieuwWachtwoord); // Update met nieuw wachtwoord
                }
                else
                {
                    BedrijfData.Update(bedrijf, null); // Update zonder wachtwoord
                }

                MessageBox.Show("Wijzigingen opgeslagen.", "Bevestiging", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                txtFeedback.Text = "Fout: " + ex.Message;
            }
        }

        private void BtnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
