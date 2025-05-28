using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Data;

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

        /// <summary>
        /// Laadt alle bedrijven met status 'aangevraagd', 'nieuw' of 'pending'.
        /// </summary>
        private void LaadAanvragen()
        {
            txtFeedback.Text = "";
            aanvragen = new List<Bedrijf>();
            try
            {
                aanvragen.AddRange(BedrijfData.GetByStatus("aangevraagd"));
                aanvragen.AddRange(BedrijfData.GetByStatus("nieuw"));
                aanvragen.AddRange(BedrijfData.GetByStatus("pending"));
                lstAanvragen.ItemsSource = null;
                lstAanvragen.ItemsSource = aanvragen;
                if (aanvragen.Count == 0)
                {
                    txtFeedback.Text = "Er zijn momenteel geen nieuwe aanvragen.";
                }
            }
            catch (Exception ex)
            {
                txtFeedback.Text = "Fout bij laden van aanvragen: " + ex.Message;
            }
            ToonDetail(null);
        }

        /// <summary>
        /// Toont de details van het geselecteerde bedrijf, of wist alles als er geen selectie is.
        /// </summary>
        /// <param name="bedrijf"></param>
        private void ToonDetail(Bedrijf bedrijf)
        {
            if (bedrijf != null)
            {
                lblNaam.Content = bedrijf.Naam;
                lblContactpersoon.Content = bedrijf.Contactpersoon;
                lblAdres.Content = bedrijf.Adres;
                lblPostcode.Content = bedrijf.Postcode;
                lblGemeente.Content = bedrijf.Gemeente;
                lblEmail.Content = bedrijf.Email;
                lblStatus.Content = bedrijf.Status;

                if (bedrijf.Logo != null && bedrijf.Logo.Length > 0)
                {
                    BitmapImage bitmap = new BitmapImage();
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream(bedrijf.Logo))
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
                lblEmail.Content = "";
                lblStatus.Content = "";
                imgLogo.Source = null;
            }
        }

        private void lstAanvragen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bedrijf bedrijf = lstAanvragen.SelectedItem as Bedrijf;
            ToonDetail(bedrijf);
        }

        /// <summary>
        /// Keurt een aanvraag goed (status naar 'actief').
        /// </summary>
        /// <param name="bedrijfId"></param>
        private void KeurtAanvraagGoed(int bedrijfId)
        {
            try
            {
                BedrijfData.UpdateStatus(bedrijfId, "actief");
                MessageBox.Show("Aanvraag goedgekeurd.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                LaadAanvragen();
            }
            catch (Exception ex)
            {
                txtFeedback.Text = "Fout bij goedkeuren: " + ex.Message;
            }
        }

        /// <summary>
        /// Weigert een aanvraag (status naar 'geweigerd').
        /// </summary>
        /// <param name="bedrijfId"></param>
        private void WeigerAanvraag(int bedrijfId)
        {
            try
            {
                BedrijfData.UpdateStatus(bedrijfId, "geweigerd");
                MessageBox.Show("Aanvraag geweigerd.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                LaadAanvragen();
            }
            catch (Exception ex)
            {
                txtFeedback.Text = "Fout bij weigeren: " + ex.Message;
            }
        }

        private void btnGoedkeuren_Click(object sender, RoutedEventArgs e)
        {
            Bedrijf bedrijf = lstAanvragen.SelectedItem as Bedrijf;
            if (bedrijf == null)
            {
                txtFeedback.Text = "Selecteer eerst een aanvraag.";
                return;
            }
            KeurtAanvraagGoed(bedrijf.Id);
        }

        private void btnWeigeren_Click(object sender, RoutedEventArgs e)
        {
            Bedrijf bedrijf = lstAanvragen.SelectedItem as Bedrijf;
            if (bedrijf == null)
            {
                txtFeedback.Text = "Selecteer eerst een aanvraag.";
                return;
            }
            WeigerAanvraag(bedrijf.Id);
        }
    }
}
