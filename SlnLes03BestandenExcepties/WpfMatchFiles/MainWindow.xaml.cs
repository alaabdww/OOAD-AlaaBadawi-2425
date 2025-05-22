using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace WpfMatchFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private List<string> LeesTriplets(string path)
        {
            HashSet<string> lijst = new HashSet<string>();
            string tekst = File.ReadAllText(path);

            // Met AI
            tekst = Regex.Replace(tekst, "[^a-zA-Z]", " ");
            tekst = Regex.Replace(tekst, @"\s+", " ");

            string[] woorden = tekst.Split(' ');

            for (int i = 0; i < woorden.Length - 2; i++)
            {
                string triplet = $"{woorden[i]} {woorden[i+1]} {woorden[i + 2]}";
                lijst.Add(triplet);
            }

            return lijst.ToList();
        }

        private double BerekenOvereenkomst(List<string> lijst1, List<string> lijst2)
        {
            HashSet<string> set1 = new HashSet<string>(lijst1);
            HashSet<string> set2 = new HashSet<string>(lijst2);

            int aantalOvereenkomsten = set1.Intersect(set2).Count();
            double totaal = set1.Count + set2.Count;

            double percentage = ((double)aantalOvereenkomsten * 2.0 / totaal) * 100;
            return Math.Round(percentage, 2);

        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            string chosenFileName;
            if (dialog.ShowDialog() == true)
            {
                chosenFileName = dialog.FileName;
                txtBestand1.Text = chosenFileName;
            }
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            string chosenFileName;
            if (dialog.ShowDialog() == true)
            {
                chosenFileName = dialog.FileName;
                txtBestand2.Text = chosenFileName;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string bestand1 = txtBestand1.Text;
            string bestand2 = txtBestand2.Text;

            bool bestaat = File.Exists(bestand1) && File.Exists(bestand2);
            if (bestaat == true)
            {
                List<string> file1 = LeesTriplets(bestand1);
                List<string> file2 = LeesTriplets(bestand2);

                double percentage = BerekenOvereenkomst(file1, file2);
                txtResultaat1.Text = $"Overeenkomst: {percentage}%";
            }
            else
            {
                MessageBox.Show("Een van de bestanden bestaat niet.");
            }
        }
    }
}
