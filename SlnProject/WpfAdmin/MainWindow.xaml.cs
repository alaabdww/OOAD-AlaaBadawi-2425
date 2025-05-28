using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Data;

namespace WpfAdmin
{
    public partial class MainWindow : Window
    {
        private List<Bedrijf> bedrijven;

        public MainWindow()
        {
            InitializeComponent();
            LaadBedrijven();
        }

        private void LaadBedrijven()
        {
            bedrijven = BedrijfData.GetAll();
            lstBedrijven.ItemsSource = bedrijven;
        }

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

                if (bedrijf.Logo != null && bedrijf.Logo.Length > 0)
                {
                    BitmapImage logo = new BitmapImage();
                    using (var stream = new System.IO.MemoryStream(bedrijf.Logo))
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

        private void BtnNieuw_Click(object sender, RoutedEventArgs e)
        {
            BedrijfNieuwWindow nieuwWindow = new BedrijfNieuwWindow();
            if (nieuwWindow.ShowDialog() == true)
            {
                LaadBedrijven();
            }
        }

        private void BtnBewerk_Click(object sender, RoutedEventArgs e)
        {
            Bedrijf bedrijf = lstBedrijven.SelectedItem as Bedrijf;
            if (bedrijf == null)
            {
                MessageBox.Show("Selecteer eerst een bedrijf.");
                return;
            }
            BedrijfEditWindow editWindow = new BedrijfEditWindow(bedrijf);
            if (editWindow.ShowDialog() == true)
            {
                LaadBedrijven();
            }
        }

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
    }
}
