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
        private byte[] logoBytes = null;

        public BedrijfNieuwWindow()
        {
            InitializeComponent();
        }

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

        private void BtnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            txtFeedback.Text = "";
            try
            {
                Bedrijf bedrijf = new Bedrijf();
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
                bedrijf.Wachtwoord = pwdWachtwoord.Password;
                bedrijf.Registratiedatum = DateTime.Now;
                bedrijf.Acceptatiedatum = null;
                bedrijf.LaatstGewijzigd = DateTime.Now;
                bedrijf.Status = "nieuw"; // altijd nieuw voor nieuwe bedrijven
                bedrijf.Taal = txtTaal.Text.Trim();
                bedrijf.Logo = logoBytes;
                bedrijf.Nacecode = txtNacecode.Text.Trim();

                // Vereiste velden controleren
                if (string.IsNullOrWhiteSpace(bedrijf.Naam) ||
                    string.IsNullOrWhiteSpace(bedrijf.Login) ||
                    string.IsNullOrWhiteSpace(bedrijf.Wachtwoord))
                {
                    txtFeedback.Text = "Vul minimaal naam, login en wachtwoord in!";
                    return;
                }

                BedrijfData.Insert(bedrijf);

                txtFeedback.Foreground = System.Windows.Media.Brushes.Green;
                txtFeedback.Text = "Bedrijf succesvol toegevoegd!";
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                txtFeedback.Foreground = System.Windows.Media.Brushes.Red;
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
