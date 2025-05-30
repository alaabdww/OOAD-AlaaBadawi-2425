// BenchmarkPage.cs
// ----------------
// Deze pagina toont het benchmarkoverzicht van een bedrijf voor een bepaald jaar, met staafdiagram per kostencategorie.
// De gebruiker kan het jaar kiezen, het diagram bekijken en terug naar home navigeren.

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using BenchmarkToolLibrary.Data;

namespace WpfUser
{
    public partial class BenchmarkPage : Page
    {
        private int bedrijfId;      // Id van het ingelogde bedrijf
        private Frame frame;        // Referentie naar het hoofdframe voor navigatie

        /// <summary>
        /// Constructor. Zet bedrijfId en laadt beschikbare jaren.
        /// </summary>
        public BenchmarkPage(Frame frame)
        {
            InitializeComponent();
            this.frame = frame;
            if (Application.Current.Properties["BedrijfId"] != null)
                bedrijfId = (int)Application.Current.Properties["BedrijfId"];
            else
                bedrijfId = 1; // fallback: eerste bedrijf als default
            LaadJaren();
        }

        /// <summary>
        /// Haalt alle jaren op waarvoor het bedrijf rapporten heeft, en vult de jaar-combobox.
        /// </summary>
        private void LaadJaren()
        {
            cmbJaar.Items.Clear();
            List<int> jaren = RapportData.GetJarenVoorBedrijf(bedrijfId);
            foreach (int jaar in jaren)
                cmbJaar.Items.Add(jaar);

            if (cmbJaar.Items.Count > 0)
                cmbJaar.SelectedIndex = 0; // selecteer automatisch het eerste jaar
        }

        /// <summary>
        /// Haalt en toont de benchmarkgegevens voor het geselecteerde jaar.
        /// </summary>
        private void ToonBenchmark()
        {
            if (cmbJaar.SelectedItem == null)
            {
                lblFeedback.Text = "Selecteer een jaartal.";
                return;
            }
            int jaar = (int)cmbJaar.SelectedItem;
            List<BenchmarkResultaat> data = RapportData.GetVergelijking(bedrijfId, jaar);
            ToonBenchmarkStaafdiagram(data);
        }

        /// <summary>
        /// Tekent het staafdiagram voor de opgegeven benchmarkdata (kost per categorie).
        /// </summary>
        private void ToonBenchmarkStaafdiagram(List<BenchmarkResultaat> data)
        {
            staafDiagramPanel.Children.Clear();
            labelsPanel.Children.Clear();

            // Bepaal de hoogste waarde om te normaliseren
            double max = 0.0;
            foreach (BenchmarkResultaat item in data)
                if (item.Waarde > max) max = item.Waarde;
            if (max == 0) max = 1.0;

            // Teken voor elke categorie een oranje staaf en bijhorend label
            foreach (BenchmarkResultaat item in data)
            {
                double hoogte = 180 * (item.Waarde / max);
                Rectangle staaf = new Rectangle();
                staaf.Width = 40;
                staaf.Height = hoogte;
                staaf.Fill = new SolidColorBrush(Colors.Orange);
                staaf.Margin = new Thickness(8, 0, 8, 0);
                staaf.RadiusX = 8;
                staaf.RadiusY = 8;
                staafDiagramPanel.Children.Add(staaf);

                TextBlock label = new TextBlock();
                label.Text = item.CategorieNaam;
                label.Width = 56;
                label.TextAlignment = TextAlignment.Center;
                label.Margin = new Thickness(2, 0, 2, 0);
                labelsPanel.Children.Add(label);
            }
            lblFeedback.Text = "";
        }

        /// <summary>
        /// Wordt getriggerd als het geselecteerde jaar verandert.
        /// </summary>
        private void cmbJaar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToonBenchmark();
        }

        /// <summary>
        /// Wordt getriggerd bij klikken op de "Filter"-knop (optioneel, laadt de benchmark opnieuw).
        /// </summary>
        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            lblFeedback.Text = "Benchmark wordt geladen...";
            ToonBenchmark();
        }

        /// <summary>
        /// Navigeert terug naar het hoofdmenu.
        /// </summary>
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new DashboardPage(frame);
        }
    }
}
