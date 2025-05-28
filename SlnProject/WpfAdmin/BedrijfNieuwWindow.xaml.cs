using BenchmarkToolLibrary.Data;
using BenchmarkToolLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                Bedrijf bedrijf = new Bedrijf
                {
                    Naam = txtNaam.Text.Trim(),
                    Contactpersoon = txtContactpersoon.Text.Trim(),
                    Adres = txtAdres.Text.Trim(),
                    Postcode = txtPostcode.Text.Trim(),
                    Gemeente = txtGemeente.Text.Trim(),
                    Land = txtLand.Text.Trim(),
                    Telefoon = txtTelefoon.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    BtwNummer = txtBtwNummer.Text.Trim(),
                    Login = txtLogin.Text.Trim(),
                    Wachtwoord = pwdWachtwoord.Password,
                    Registratiedatum = DateTime.Now,
                    Acceptatiedatum = null,
                    LaatstGewijzigd = DateTime.Now,
                    Status = ((ComboBoxItem)cmbStatus.SelectedItem).Content.ToString(),
                    Taal = txtTaal.Text.Trim(),
                    Logo = logoBytes,
                    Nacecode = txtNacecode.Text.Trim()
                };

                if (string.IsNullOrWhiteSpace(bedrijf.Naam) ||
                    string.IsNullOrWhiteSpace(bedrijf.Login) ||
                    string.IsNullOrWhiteSpace(bedrijf.Wachtwoord))
                {
                    txtFeedback.Text = "Vul minimaal naam, login en wachtwoord in!";
                    return;
                }

                BedrijfData.Insert(bedrijf);
                MessageBox.Show("Bedrijf succesvol toegevoegd!", "Bevestiging", MessageBoxButton.OK, MessageBoxImage.Information);
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
