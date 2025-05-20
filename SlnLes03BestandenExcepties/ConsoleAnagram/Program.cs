using System;
using System.Collections.Generic;
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
            Console.WriteLine("ANAGRAM");
            Console.WriteLine("=======");
            Console.Write("Kies het aantal letters: ");
            int aantalLetters = int.Parse(Console.ReadLine());
            string filePath = "Bestanden/1000woorden.txt";

            string[] regels = File.ReadAllLines(filePath); // lees regels bestand in
            Console.WriteLine($"Aantal woorden in bestand: {regels.Length}");
            Console.ReadKey();
        }
    }
}
