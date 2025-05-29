using System.Windows;
using System.Windows.Controls;
using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Data;

namespace WpfUser
{
    public partial class ProfielPage : Page
    {
        private Frame frame;
        private Bedrijf bedrijf;

        public ProfielPage(Frame frame)
        {
            InitializeComponent();
            this.frame = frame;
            int bedrijfId = (int)Application.Current.Properties["BedrijfId"];
            bedrijf = BedrijfData.GetById(bedrijfId);

            txtNaam.Text = bedrijf.Naam;
            txtContact.Text = bedrijf.Contactpersoon;
            txtAdres.Text = bedrijf.Adres;
            txtPostcode.Text = bedrijf.Postcode;
            txtGemeente.Text = bedrijf.Gemeente;
            txtEmail.Text = bedrijf.Email;
            txtTaal.Text = bedrijf.Taal;
            // Logo kan hier toegevoegd worden als je wilt (optioneel)
        }

        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            bedrijf.Naam = txtNaam.Text;
            bedrijf.Contactpersoon = txtContact.Text;
            bedrijf.Adres = txtAdres.Text;
            bedrijf.Postcode = txtPostcode.Text;
            bedrijf.Gemeente = txtGemeente.Text;
            bedrijf.Email = txtEmail.Text;
            bedrijf.Taal = txtTaal.Text;

            string nieuwWachtwoord = "";
            if (!string.IsNullOrWhiteSpace(pwdWachtwoord.Password))
            {
                nieuwWachtwoord = pwdWachtwoord.Password;
            }
            BedrijfData.Update(bedrijf, nieuwWachtwoord);

            txtFeedback.Text = "Wijzigingen opgeslagen.";
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new DashboardPage(frame);
        }
    }
}
