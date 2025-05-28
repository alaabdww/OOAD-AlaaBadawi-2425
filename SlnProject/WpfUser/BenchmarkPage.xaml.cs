using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using BenchmarkToolLibrary.Data;
using BenchmarkToolLibrary.Models;

namespace WpfUser
{
    public partial class BenchmarkPage : Page
    {
        private int bedrijfId;
        private Frame frame;

        public BenchmarkPage(Frame frame)
        {
            InitializeComponent();
            this.frame = frame;
            if (Application.Current.Properties["BedrijfId"] != null)
                bedrijfId = (int)Application.Current.Properties["BedrijfId"];
            else
                bedrijfId = 1;
            LaadJaren();
        }

        private void LaadJaren()
        {
            cmbJaar.Items.Clear();
            List<int> jaren = RapportData.GetJarenVoorBedrijf(bedrijfId);
            foreach (int jaar in jaren)
                cmbJaar.Items.Add(jaar);

            if (cmbJaar.Items.Count > 0)
                cmbJaar.SelectedIndex = 0;
        }

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

        private void ToonBenchmarkStaafdiagram(List<BenchmarkResultaat> data)
        {
            staafDiagramPanel.Children.Clear();
            labelsPanel.Children.Clear();

            double max = 0.0;
            foreach (BenchmarkResultaat item in data)
                if (item.Waarde > max) max = item.Waarde;
            if (max == 0) max = 1.0;

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

        private void cmbJaar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ToonBenchmark();
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            lblFeedback.Text = "Benchmark wordt geladen...";
            ToonBenchmark();
        }
    }

    public class BenchmarkResultaat
    {
        public string CategorieNaam { get; set; }
        public double Waarde { get; set; }
    }

    public static class RapportData
    {
        public static List<int> GetJarenVoorBedrijf(int bedrijfId)
        {
            return new List<int> { 2022, 2023 };
        }

        public static List<BenchmarkResultaat> GetVergelijking(int bedrijfId, int jaar)
        {
            return new List<BenchmarkResultaat>
            {
                new BenchmarkResultaat { CategorieNaam = "Personeel", Waarde = 10000 },
                new BenchmarkResultaat { CategorieNaam = "Materiaal", Waarde = 6000 },
                new BenchmarkResultaat { CategorieNaam = "ICT", Waarde = 4500 },
                new BenchmarkResultaat { CategorieNaam = "Kantoor", Waarde = 3200 },
                new BenchmarkResultaat { CategorieNaam = "Overig", Waarde = 1800 }
            };
        }

        internal static void Delete(int id) { }
        internal static List<Jaarrapport> GetAllVoorBedrijf(int bedrijfId) { return new List<Jaarrapport>(); }
    }
}
