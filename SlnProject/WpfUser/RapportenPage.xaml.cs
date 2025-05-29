using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Data;

namespace WpfUser
{
    public partial class RapportenPage : Page
    {
        private Frame frame;
        private List<Jaarrapport> rapporten;
        private int bedrijfId;

        public RapportenPage(Frame frame)
        {
            InitializeComponent();
            this.frame = frame;

            if (Application.Current.Properties["BedrijfId"] != null)
            {
                bedrijfId = (int)Application.Current.Properties["BedrijfId"];
            }
            else
            {
                MessageBox.Show("Geen bedrijfId gevonden in session. Gelieve opnieuw in te loggen.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                bedrijfId = -1;
            }

            LaadRapporten();
        }

        private void LaadRapporten()
        {
            txtFeedback.Text = "";
            rapporten = new List<Jaarrapport>();
            if (bedrijfId != -1)
            {
                rapporten = RapportData.GetAllVoorBedrijf(bedrijfId);
            }
            lstRapporten.ItemsSource = null;
            lstRapporten.ItemsSource = rapporten;
            txtRapportDetail.Text = "";
        }

        private void lstRapporten_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Jaarrapport rapport = lstRapporten.SelectedItem as Jaarrapport;
            if (rapport != null)
            {
                txtRapportDetail.Text =
                    "Jaar: " + rapport.Jaar +
                    "\nStatus: " + rapport.Status +
                    "\nDatum: " + rapport.Rapportdatum.ToShortDateString();
            }
            else
            {
                txtRapportDetail.Text = "";
            }
        }

        private void btnNieuw_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Nieuw rapport functionaliteit moet nog geïmplementeerd worden.");
        }

        private void btnBewerk_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Bewerk rapport functionaliteit moet nog geïmplementeerd worden.");
        }

        private void btnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            Jaarrapport rapport = lstRapporten.SelectedItem as Jaarrapport;
            if (rapport == null)
            {
                txtFeedback.Text = "Selecteer eerst een rapport.";
                return;
            }
            if (MessageBox.Show("Ben je zeker dat je dit rapport wil verwijderen?", "Bevestigen", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                try
                {
                    RapportData.Delete(rapport.Id);
                    txtFeedback.Text = "Rapport succesvol verwijderd.";
                }
                catch
                {
                    txtFeedback.Text = "Fout bij het verwijderen van het rapport.";
                }
                LaadRapporten();
            }
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new DashboardPage(frame);
        }
    }
}
