// MainWindow.xaml.cs
// ------------------
// Dit is het hoofdscherm voor de admin in WpfAdmin. Hier beheer je alle bedrijven:
// - Lijst tonen
// - Details bekijken
// - Toevoegen/bewerken/verwijderen van bedrijven
// - Registratieaanvragen goedkeuren/weigeren

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Data;

namespace WpfAdmin
{
    public partial class MainWindow : Window
    {
        private List<Bedrijf> bedrijven;

        /// <summary>
        /// Constructor: initialiseert het hoofdvenster en laadt alle bedrijven.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            LaadBedrijven();
        }

        /// <summary>
        /// Haalt de lijst van bedrijven op uit de database en vult de ListBox.
        /// </summary>
        private void LaadBedrijven()
        {
            bedrijven = BedrijfData.GetAll();
            lstBedrijven.ItemsSource = null; // Even leegmaken om te forceren dat UI refresht
            lstBedrijven.ItemsSource = bedrijven;
        }

        /// <summary>
        /// Wordt aangeroepen als een ander bedrijf geselecteerd wordt.
        /// Toont de details en het logo van het geselecteerde bedrijf.
        /// </summary>
        private void lstBedrijven_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Bedrijf bedrijf = lstBedrijven.SelectedItem as Bedrijf;
            if (bedrijf != null)
            {
                lblNaam.Content = bedrijf.Naam;
                lblContactpersoon.Content = bedrijf.Contactpersoon;
                lblAdres.Content = bedrijf.Adres;
                lblPostcode.Content = bedrijf.Postcode;
                lblGemeente.Content = bedrijf.Gemeente;
                lblLand.Content = bedrijf.Land;
                lblTelefoon.Content = bedrijf.Telefoon;
                lblEmail.Content = bedrijf.Email;
                lblStatus.Content = bedrijf.Status;

                // Toon logo als er één aanwezig is
                if (bedrijf.Logo != null && bedrijf.Logo.Length > 0)
                {
                    BitmapImage logo = new BitmapImage();
                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream(bedrijf.Logo))
                    {
                        logo.BeginInit();
                        logo.CacheOption = BitmapCacheOption.OnLoad;
                        logo.StreamSource = stream;
                        logo.EndInit();
                    }
                    imgLogo.Source = logo;
                }
                else
                {
                    imgLogo.Source = null;
                }
            }
            else
            {
                // Maak alles leeg als niets geselecteerd
                lblNaam.Content = "";
                lblContactpersoon.Content = "";
                lblAdres.Content = "";
                lblPostcode.Content = "";
                lblGemeente.Content = "";
                lblLand.Content = "";
                lblTelefoon.Content = "";
                lblEmail.Content = "";
                lblStatus.Content = "";
                imgLogo.Source = null;
            }
        }

        /// <summary>
        /// Opent een venster om een nieuw bedrijf toe te voegen.
        /// </summary>
        private void BtnNieuw_Click(object sender, RoutedEventArgs e)
        {
            BedrijfNieuwWindow nieuwWindow = new BedrijfNieuwWindow();
            if (nieuwWindow.ShowDialog() == true)
            {
                LaadBedrijven();
            }
        }

        /// <summary>
        /// Opent het bewerkingsvenster voor het geselecteerde bedrijf (met kopie!).
        /// Bij opslaan worden wijzigingen overgenomen.
        /// </summary>
        private void BtnBewerk_Click(object sender, RoutedEventArgs e)
        {
            Bedrijf origineel = lstBedrijven.SelectedItem as Bedrijf;
            if (origineel == null)
            {
                MessageBox.Show("Selecteer eerst een bedrijf.");
                return;
            }
            // Bewerk een kopie, zodat annuleren geen gevolgen heeft
            Bedrijf kopie = origineel.Clone();
            BedrijfEditWindow editWindow = new BedrijfEditWindow(kopie);
            if (editWindow.ShowDialog() == true)
            {
                // Neem wijzigingen over naar het originele object
                origineel.Naam = kopie.Naam;
                origineel.Contactpersoon = kopie.Contactpersoon;
                origineel.Adres = kopie.Adres;
                origineel.Postcode = kopie.Postcode;
                origineel.Gemeente = kopie.Gemeente;
                origineel.Land = kopie.Land;
                origineel.Telefoon = kopie.Telefoon;
                origineel.Email = kopie.Email;
                origineel.BtwNummer = kopie.BtwNummer;
                origineel.Taal = kopie.Taal;
                origineel.Status = kopie.Status;
                origineel.Logo = kopie.Logo;
                origineel.Nacecode = kopie.Nacecode;
                origineel.LaatstGewijzigd = kopie.LaatstGewijzigd;
                LaadBedrijven();
            }
        }

        /// <summary>
        /// Verwijdert het geselecteerde bedrijf na bevestiging.
        /// </summary>
        private void BtnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            Bedrijf bedrijf = lstBedrijven.SelectedItem as Bedrijf;
            if (bedrijf == null)
            {
                MessageBox.Show("Selecteer eerst een bedrijf om te verwijderen.");
                return;
            }
            if (MessageBox.Show("Weet je zeker dat je dit bedrijf wilt verwijderen?", "Bevestig", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    BedrijfData.Delete(bedrijf.Id);
                    LaadBedrijven();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fout bij verwijderen: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Opent het venster voor registratieaanvragen en herlaadt bedrijven achteraf.
        /// </summary>
        private void BtnRegistratieAanvragen_Click(object sender, RoutedEventArgs e)
        {
            RegistratieAanvragenWindow aanvragenWindow = new RegistratieAanvragenWindow();
            aanvragenWindow.Owner = this;
            aanvragenWindow.ShowDialog();
            LaadBedrijven();
        }
    }
}
