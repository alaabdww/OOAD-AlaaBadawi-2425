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
using System.Windows.Threading;

namespace WpfEllipsen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Random rnd = new Random();
        DispatcherTimer timer = new DispatcherTimer();
        int elipscount = 0;
        int maxelips = 25;
        int minsize = 20;
        int maxsize = 100;
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += Timer_Tick;
            // maak de ellips
            Ellipse newEllipse = new Ellipse()
            {
                Width = 150,
                Height = 60,
                Fill = new SolidColorBrush(Color.FromRgb(122, 78, 200))
            };
            double xPos = 50;
            double yPos = 85;
            newEllipse.SetValue(Canvas.LeftProperty, xPos);
            newEllipse.SetValue(Canvas.TopProperty, yPos);
            //voeg ellips toe aan het canvas
            canvas1.Children.Add(newEllipse);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (elipscount > maxelips)
            {
                timer.Stop();
                return;
            }
            // maak de ellips
            else
            {
                Ellipse newEllipse = new Ellipse()
                {
                    Width = rnd.Next(minsize, maxsize + 1),
                    Height = rnd.Next(minsize, maxsize + 1),
                    Fill = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256), (byte)rnd.Next(0, 256)))
                };
                double xPos = rnd.Next(0, 700);
                double yPos = rnd.Next(0, 255);
                newEllipse.SetValue(Canvas.LeftProperty, xPos);
                newEllipse.SetValue(Canvas.TopProperty, yPos);
                canvas1.Children.Add(newEllipse);
                elipscount++;
            }
        }

        private void btnTekenen_Click(object sender, RoutedEventArgs e)
        {
            elipscount = 0;
            canvas1.Children.Clear();
            timer.Start();

        }
    }
}
