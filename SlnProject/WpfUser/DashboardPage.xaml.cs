// DashboardPage.xaml.cs
// ---------------------
// Dit is het dashboard voor een bedrijf/gebruiker in WpfUser.
// Hier wordt een welkomstbericht en het logo getoond, en kan je navigeren naar rapporten, benchmarking, profiel of uitloggen.

using System.Windows;
using System.Windows.Controls;
using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Data;
using System.Windows.Media.Imaging;
using System.IO;

namespace WpfUser
{
    public partial class DashboardPage : Page
    {
        private Frame frame;
        private Bedrijf bedrijf;

        /// <summary>
        /// Initialiseert het dashboard, toont het welkomstbericht en logo.
        /// </summary>
        public DashboardPage(Frame frame)
        {
            InitializeComponent();
            this.frame = frame;

            // Haal huidig bedrijf op via App properties (ingelogd bedrijf)
            int bedrijfId = (int)Application.Current.Properties["BedrijfId"];
            bedrijf = BedrijfData.GetById(bedrijfId);

            txtWelkom.Text = "Welkom, " + bedrijf.Naam + "!";

            // Toon logo indien beschikbaar
            if (bedrijf.Logo != null && bedrijf.Logo.Length > 0)
            {
                BitmapImage bitmap = new BitmapImage();
                using (MemoryStream ms = new MemoryStream(bedrijf.Logo))
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                }
                imgLogo.Source = bitmap;
            }
        }

        /// <summary>
        /// Navigeer naar het rapportenoverzicht.
        /// </summary>
        private void btnRapporten_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new RapportenPage(frame);
        }

        /// <summary>
        /// Navigeer naar de benchmarkingpagina.
        /// </summary>
        private void btnBenchmark_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new BenchmarkPage(frame);
        }

        /// <summary>
        /// Navigeer naar de profielpagina.
        /// </summary>
        private void btnProfiel_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new ProfielPage(frame);
        }

        /// <summary>
        /// Log uit en keer terug naar het login scherm.
        /// </summary>
        private void btnUitloggen_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["BedrijfId"] = null;
            frame.Content = new LoginPage(frame);
        }
    }
}
