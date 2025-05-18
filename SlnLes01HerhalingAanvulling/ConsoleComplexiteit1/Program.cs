using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleComplexiteit1
{
    internal class Program
    {
        private static bool IsKlinker(char C)
        {
            C = char.ToLower(C);
            return C == 'a' || C == 'e' || C == 'i' || C == 'o' || C == 'u' || C == 'y';
        } 

        private static int AantalLettergrepen(string a)
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

        private static double Complexiteit(string a)
        {
            double complexiteit = 0;
            int lettergrepen= AantalLettergrepen(a);
            double aantalLetters = Convert.ToDouble(a.Length);
            for (int i = 0; i < a.Length; i++)
            {
                char C = char.ToLower(a[i]);
                if (C == 'x' || C == 'y' || C == 'z')
                {
                    complexiteit++;
                }

            }
            complexiteit = complexiteit + (aantalLetters / 3) + lettergrepen;
            return complexiteit;
        }
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Geef een woord (enter om te stoppen):");
                string woord = Console.ReadLine();
                if (woord == "")
                    break;
                Console.WriteLine($"aantal karakters: {woord.Length}");
                Console.WriteLine($"aantal lettergrepen: {AantalLettergrepen(woord)}");
                Console.WriteLine($"complexiteit: {Complexiteit(woord)}");
                Console.WriteLine(" ");
            }

        }
    }
}
