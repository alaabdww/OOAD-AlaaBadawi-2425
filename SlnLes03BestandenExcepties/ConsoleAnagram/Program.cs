using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAnagram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string gekozenWoord;
            Console.WriteLine("ANAGRAM");
            Console.WriteLine("=======");
            Console.Write("Kies het aantal letters (5-15): ");
            int aantalLetters = int.Parse(Console.ReadLine());
            string filePath = "Bestanden/1000woorden.txt";

            string[] regels = File.ReadAllLines(filePath); // lees regels bestand in

            // Filter de woorden op lengte
            List<string> juisteWoorden = regels.Where(woord => woord.Length == aantalLetters).ToList();
            Random rnd = new Random();
            gekozenWoord = juisteWoorden[rnd.Next(0, juisteWoorden.Count)];

            // Maak een anagram van het gekozen woord
            char[] letters = gekozenWoord.ToCharArray();
            letters = letters.OrderBy(x => rnd.Next()).ToArray();
            string anagram = new string(letters);
            Console.WriteLine(" ");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            bool isCorrect = false;

            while (isCorrect == false)
            {
                Console.WriteLine($"Anagram: {anagram}");
                Console.Write("Het woord (druk op enter om opnieuw te schudden): ");
                string invoer = Console.ReadLine();
                if (invoer == null || invoer == "")
                {
                    letters = letters.OrderBy(x => rnd.Next()).ToArray();
                    anagram = new string(letters);
                }
                else if (invoer.ToLower() == gekozenWoord.ToLower())
                {
                    stopwatch.Stop();
                    isCorrect = true;
                    TimeSpan tijd = stopwatch.Elapsed;
                    Console.WriteLine($"Gefeliciteerd! Je hebt het woord {gekozenWoord} geraden in {tijd.Minutes} minuten en {tijd.Seconds} seconden.");
                }
                else
                {
                    Console.WriteLine($"Helaas het woord was {gekozenWoord} ");
                    stopwatch.Stop();
                    isCorrect = true;
                }

            }


            Console.ReadKey();
        }
    }
}
