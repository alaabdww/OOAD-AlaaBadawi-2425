using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;
using System.Media;


namespace WpfRaadLand
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Random rnd = new Random();
        DispatcherTimer timer = new DispatcherTimer();
        Stopwatch stopwatch = new Stopwatch();
        string GekozenLand;
        List<string> landen = new List<string>
        {
            "finland",
            "marokko",
            "japan",
            "argentina",
            "nieuwzeeland",
        };

        string land;
        int aantalOef = 0;
        int juistOef = 0;
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick (object sender, EventArgs e)
        {
            double maxBreedte = 500;
            double percentage = stopwatch.ElapsedMilliseconds / 5000.0;
            double nieuweBreedte = maxBreedte * (1 - percentage);

            if (nieuweBreedte < 1)
            {
                nieuweBreedte = 0.1;
            }
            tijdsbalk.Width = nieuweBreedte;

            if (stopwatch.ElapsedMilliseconds >= 5000)
            {
                timer.Stop();
                stopwatch.Stop();
                startBtn.IsEnabled = true;
                ResultaatTxt.Text = $"Fout! Tijd is op. Je hebt {juistOef}/{aantalOef}. Druk op start!";
            }


        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {

            Image gekozenland = (Image)sender;
            string naamGekozenLand = gekozenland.Tag.ToString();

            if (naamGekozenLand == land)
            {
                timer.Stop();
                stopwatch.Stop();
                startBtn.IsEnabled = true;
                juistOef++;
                double gemTijd = Math.Round(stopwatch.ElapsedMilliseconds / 1000.0, 1);
                ResultaatTxt.Text = $"Juist! Je hebt {juistOef}/{aantalOef}. Je gemiddelde tijd is {gemTijd} Druk op start!";
            }
            else
            {
                timer.Stop();
                stopwatch.Stop();
                startBtn.IsEnabled = true;
                ResultaatTxt.Text = $"Fout! Je hebt {juistOef}/{aantalOef}. Druk op start!";
            }


        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            stopwatch.Reset();
            int index = rnd.Next(0, landen.Count);
            GekozenLand = landen[index];
            land = GekozenLand;
            aantalOef++;
            ResultaatTxt.Text = $"Duid {GekozenLand} aan!";
            startBtn.IsEnabled = false;
            stopwatch.Start();
            timer.Start();

        }
    }
}
