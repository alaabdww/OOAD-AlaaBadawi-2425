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
        private List<Bedrijf> aanvragen;

        public RegistratieAanvragenWindow()
        {
            InitializeComponent();
            LaadAanvragen();
        }

        private void LaadAanvragen()
        {
            aanvragen = BedrijfData.GetByStatus("nieuw");
            lstAanvragen.ItemsSource = aanvragen;
            lstAanvragen.SelectedIndex = -1;

            // Reset detailvelden
            ToonDetails(null);
        }

        private void lstAanvragen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bedrijf bedrijf = lstAanvragen.SelectedItem as Bedrijf;
            ToonDetails(bedrijf);
        }

        private void ToonDetails(Bedrijf bedrijf)
        {
            if (bedrijf != null)
            {
                lblNaam.Content = bedrijf.Naam;
                lblContactpersoon.Content = bedrijf.Contactpersoon;
                lblAdres.Content = bedrijf.Adres;
                lblPostcode.Content = bedrijf.Postcode;
                lblGemeente.Content = bedrijf.Gemeente;
                lblLogin.Content = bedrijf.Login;
                lblEmail.Content = bedrijf.Email;
                lblStatus.Content = bedrijf.Status;

                if (bedrijf.Logo != null && bedrijf.Logo.Length > 0)
                {
                    BitmapImage bitmap = new BitmapImage();
                    using (MemoryStream ms = new MemoryStream(bedrijf.Logo))
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

        private void btnGoedkeuren_Click(object sender, RoutedEventArgs e)
        {
            Bedrijf bedrijf = lstAanvragen.SelectedItem as Bedrijf;
            if (bedrijf == null)
            {
                txtFeedback.Text = "Selecteer een aanvraag.";
                return;
            }
            BedrijfData.UpdateStatus(bedrijf.Id, "actief");
            LaadAanvragen();
        }

        private void btnWeigeren_Click(object sender, RoutedEventArgs e)
        {
            Bedrijf bedrijf = lstAanvragen.SelectedItem as Bedrijf;
            if (bedrijf == null)
            {
                txtFeedback.Text = "Selecteer een aanvraag.";
                return;
            }
            BedrijfData.UpdateStatus(bedrijf.Id, "geweigerd");
            LaadAanvragen();
        }
    }
}
