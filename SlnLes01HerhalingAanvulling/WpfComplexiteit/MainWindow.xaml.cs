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

namespace WpfComplexiteit
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

        private bool IsKlinker(char C)
        {
            C = char.ToLower(C);
            return C == 'a' || C == 'e' || C == 'i' || C == 'o' || C == 'u' || C == 'y';
        }

        private int AantalLettergrepen(string a)
        {
            int aantal = 0;
            bool vorigeKlinker = false;
            for (int i = 0; i < a.Length; i++)
            {
                if (IsKlinker(a[i]))
                {
                    if (vorigeKlinker == false)
                    {
                        aantal++;
                        vorigeKlinker = true;
                    }

                }
                if (!IsKlinker(a[i]))
                {
                    vorigeKlinker = false;
                }
            }

            return aantal;
        }

        private double Complexiteit(string a)
        {
            double complexiteit = 0;
            int lettergrepen = AantalLettergrepen(a);
            double aantalLetters = Convert.ToDouble(a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                char C = char.ToLower(a[i]);
                if (C == 'x' || C == 'y' || C == 'q')
                {
                    complexiteit++;
                }

            }
            complexiteit = complexiteit + (aantalLetters / 3) + lettergrepen;
            return Math.Round(complexiteit, 1);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string woord = WoordTxt.Text;
            int lengte = woord.Length;
            int lettergrepen = AantalLettergrepen(woord);
            double complexiteit = Complexiteit(woord);

            ResultaatTxt.Text = $"Aantal letters: {lengte}\n" +
                $"Aantal lettergrepen: {lettergrepen}\n" +
                $"Complexiteit: {complexiteit}";

        }
    }
}
