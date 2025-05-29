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
        private Bedrijf bedrijf;
        private byte[] logoBytes;

        public BedrijfEditWindow(Bedrijf bedrijf)
        {
            InitializeComponent();
            this.bedrijf = bedrijf;

            // Velden invullen met bestaande data
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

            // Status selecteren
            foreach (ComboBoxItem item in cmbStatus.Items)
            {
                if ((string)item.Content == (bedrijf.Status ?? "nieuw"))
                {
                    cmbStatus.SelectedItem = item;
                    break;
                }
            }

            // Logo tonen
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
                bedrijf.Taal = txtTaal.Text.Trim();
                bedrijf.Nacecode = txtNacecode.Text.Trim();
                bedrijf.Logo = logoBytes;
                string status = "nieuw";
                if (cmbStatus.SelectedItem is ComboBoxItem selectedStatus)
                {
                    status = selectedStatus.Content.ToString();
                }
                bedrijf.Status = status;
                bedrijf.LaatstGewijzigd = DateTime.Now;

                string nieuwWachtwoord = pwdWachtwoord.Password;
                BedrijfData.Update(bedrijf, string.IsNullOrWhiteSpace(nieuwWachtwoord) ? null : nieuwWachtwoord);

                MessageBox.Show("Bedrijf succesvol aangepast!", "Bevestiging", MessageBoxButton.OK, MessageBoxImage.Information);
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij opslaan: " + ex.Message, "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
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
