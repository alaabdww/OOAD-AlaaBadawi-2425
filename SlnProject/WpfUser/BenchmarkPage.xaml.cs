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
        private int bedrijfId;

        public BenchmarkPage(int bedrijfId)
        {
            InitializeComponent();
            this.bedrijfId = bedrijfId;
            LaadJaren();
        }

        /// <summary>
        /// Laad alle beschikbare jaartallen voor dit bedrijf in de ComboBox.
        /// </summary>
        private void LaadJaren()
        {
            cmbJaar.Items.Clear();
            List<int> jaren = RapportData.GetJarenVoorBedrijf(bedrijfId);
            foreach (int jaar in jaren)
            {
                cmbJaar.Items.Add(jaar);
            }
            if (cmbJaar.Items.Count > 0)
            {
                cmbJaar.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Toon de staafdiagram voor het geselecteerde jaar.
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
        /// Bouw de staafdiagram met Rectangle-objecten.
        /// </summary>
        private void ToonBenchmarkStaafdiagram(List<BenchmarkResultaat> data)
        {
            staafDiagramPanel.Children.Clear();
            labelsPanel.Children.Clear();

            double max = 0.0;
            foreach (BenchmarkResultaat item in data)
            {
                if (item.Waarde > max)
                {
                    max = item.Waarde;
                }
            }
            if (max == 0) { max = 1.0; }

            foreach (BenchmarkResultaat item in data)
            {
                double hoogte = 180 * (item.Waarde / max); // Max hoogte: 180px
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

        private void cmbJaar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToonBenchmark();
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            ToonBenchmark();
        }
    }

    /// <summary>
    /// Modelklasse voor een benchmarkresultaat (categorie + waarde)
    /// </summary>
    public class BenchmarkResultaat
    {
        public string CategorieNaam { get; set; }
        public double Waarde { get; set; }
    }

    /// <summary>
    /// Dummy data-access-klasse voor demonstratie.
    /// Je vervangt deze door je echte RapportData-implementatie in de class library.
    /// </summary>
    public static class RapportData
    {
        public static List<int> GetJarenVoorBedrijf(int bedrijfId)
        {
            // Dummy implementatie, vervang door echte database-call
            List<int> jaren = new List<int>();
            jaren.Add(2022);
            jaren.Add(2023);
            return jaren;
        }

        public static List<BenchmarkResultaat> GetVergelijking(int bedrijfId, int jaar)
        {
            // Dummy data voor grafiek, vervang door echte data uit de database
            List<BenchmarkResultaat> resultaten = new List<BenchmarkResultaat>();
            resultaten.Add(new BenchmarkResultaat { CategorieNaam = "Personeel", Waarde = 10000 });
            resultaten.Add(new BenchmarkResultaat { CategorieNaam = "Materiaal", Waarde = 6000 });
            resultaten.Add(new BenchmarkResultaat { CategorieNaam = "ICT", Waarde = 4500 });
            resultaten.Add(new BenchmarkResultaat { CategorieNaam = "Kantoor", Waarde = 3200 });
            resultaten.Add(new BenchmarkResultaat { CategorieNaam = "Overig", Waarde = 1800 });
            return resultaten;
        }
    }
}
