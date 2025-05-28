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

        public DashboardPage(Frame frame)
        {
            InitializeComponent();
            this.frame = frame;

            int bedrijfId = (int)Application.Current.Properties["BedrijfId"];
            bedrijf = BedrijfData.GetById(bedrijfId);

            txtWelkom.Text = "Welkom, " + bedrijf.Naam + "!";
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

        private void btnRapporten_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new RapportenPage(frame);
        }

        private void btnBenchmark_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new BenchmarkPage(frame);
        }

        private void btnProfiel_Click(object sender, RoutedEventArgs e)
        {
            frame.Content = new ProfielPage(frame);
        }

        private void btnUitloggen_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties["BedrijfId"] = null;
            frame.Content = new LoginPage(frame);
        }
    }
}
