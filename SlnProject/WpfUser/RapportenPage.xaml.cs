// RapportenPage.cs
// ------------------------
// Dit scherm toont een overzicht van de jaarrapporten van het ingelogde bedrijf.
// Functionaliteit: lijst laden, detail tonen, rapport verwijderen, en navigatie naar home.

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Data;
using System.Windows.Media;

namespace WpfUser
{
    public partial class RapportenPage : Page
    {
        private Frame frame;
        private List<Jaarrapport> rapporten;
        private int bedrijfId;

        /// <summary>
        /// Constructor: Initialiseert de pagina, haalt bedrijfId uit session, laadt de rapporten.
        /// </summary>
        public RapportenPage(Frame frame)
        {
            InitializeComponent();
            this.frame = frame;

            txtFeedback.Text = "";
            txtFeedback.Foreground = Brushes.Red;

            object idObj = Application.Current.Properties["BedrijfId"];
            if (idObj != null && idObj is int)
            {
                bedrijfId = (int)idObj;
            }
            else
            {
                txtFeedback.Text = "Fout: Geen bedrijfId gevonden in session. Gelieve opnieuw in te loggen.";
                bedrijfId = -1;
            }

            LaadRapporten();
        }

        /// <summary>
        /// Haalt alle jaarrapporten op voor het huidige bedrijf en vult de lijst.
        /// </summary>
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

        /// <summary>
        /// Toon detailinfo van het geselecteerde rapport.
        /// </summary>
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

        /// <summary>
        /// Nieuw rapport functionaliteit (nog niet geïmplementeerd).
        /// </summary>
        private void btnNieuw_Click(object sender, RoutedEventArgs e)
        {
            txtFeedback.Text = "Nieuw rapport functionaliteit moet nog geïmplementeerd worden.";
        }

        /// <summary>
        /// Bewerk rapport functionaliteit (nog niet geïmplementeerd).
        /// </summary>
        private void btnBewerk_Click(object sender, RoutedEventArgs e)
        {
            txtFeedback.Text = "Bewerk rapport functionaliteit moet nog geïmplementeerd worden.";
        }

        /// <summary>
        /// Verwijdert het geselecteerde rapport na bevestiging.
        /// </summary>
        private void btnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            txtFeedback.Text = "";
            Jaarrapport rapport = lstRapporten.SelectedItem as Jaarrapport;
            if (rapport == null)
            {
                txtFeedback.Text = "Selecteer eerst een rapport.";
                return;
            }

            // Geen MessageBox, dus directe verwijdering
            try
            {
                RapportData.Delete(rapport.Id);
                txtFeedback.Text = "Rapport succesvol verwijderd.";
                txtFeedback.Foreground = Brushes.Green;
            }
            catch
            {
                txtFeedback.Text = "Fout bij het verwijderen van het rapport.";
                txtFeedback.Foreground = Brushes.Red;
            }
            LaadRapporten();
        }

        /// <summary>
        /// Navigatie terug naar het dashboard.
        /// </summary>
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new DashboardPage(frame);
        }
    }
}
