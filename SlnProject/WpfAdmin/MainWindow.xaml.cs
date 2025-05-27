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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
            }
        }
    }
}
