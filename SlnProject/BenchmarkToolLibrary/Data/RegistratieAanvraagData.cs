// RegistratieAanvraagData.cs
// --------------------------
// Deze klasse beheert alle registratieaanvragen (zowel nieuwe bedrijven als profielwijzigingen)
// via een JSON-bestand ("aanpassingen.json") op schijf. 
// Alle CRUD-operaties gebeuren in-memory en worden direct gesynchroniseerd naar het JSON-bestand.

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using BenchmarkToolLibrary.Models;

namespace BenchmarkToolLibrary.Data
{
    public static class RegistratieAanvraagData
    {
        // Lijst van alle aanvragen in het geheugen
        private static List<RegistratieAanvraag> aanvragen = new List<RegistratieAanvraag>();
        private static int volgendeId = 1;
        // Volledig pad naar het JSON-bestand waar de aanvragen bewaard worden
        private static readonly string FilePath = @"C:\BenchmarkToolData\aanpassingen.json";

        // Statische constructor: laad aanvragen bij het opstarten van de applicatie
        static RegistratieAanvraagData()
        {
            LoadFromFile();
        }

        /// <summary>
        /// Voeg een nieuwe registratieaanvraag toe en bewaar het in het JSON-bestand.
        /// </summary>
        public static void Insert(RegistratieAanvraag aanvraag)
        {
            aanvraag.Id = volgendeId++;
            aanvraag.Status = aanvraag.Status ?? "nieuw";
            aanvraag.Registratiedatum = DateTime.Now;
            aanvragen.Add(aanvraag);
            SaveToFile();
        }

        /// <summary>
        /// Geef een lijst van alle aanvragen met status "nieuw" (openstaande aanvragen).
        /// </summary>
        public static List<RegistratieAanvraag> GetAllNieuw()
        {
            LoadFromFile(); // Zorgt altijd voor de meest recente gegevens uit bestand
            List<RegistratieAanvraag> lijst = new List<RegistratieAanvraag>();
            foreach (RegistratieAanvraag aanvraag in aanvragen)
            {
                if (aanvraag.Status == "nieuw")
                {
                    lijst.Add(aanvraag);
                }
            }
            return lijst;
        }

        /// <summary>
        /// Pas de status van een aanvraag aan (bijvoorbeeld naar 'goedgekeurd' of 'geweigerd').
        /// </summary>
        public static void UpdateStatus(int aanvraagId, string status)
        {
            for (int i = 0; i < aanvragen.Count; i++)
            {
                if (aanvragen[i].Id == aanvraagId)
                {
                    aanvragen[i].Status = status;
                    break;
                }
            }
            SaveToFile();
        }

        /// <summary>
        /// Verwijder een aanvraag uit de lijst en update het JSON-bestand.
        /// </summary>
        public static void Delete(int aanvraagId)
        {
            for (int i = 0; i < aanvragen.Count; i++)
            {
                if (aanvragen[i].Id == aanvraagId)
                {
                    aanvragen.RemoveAt(i);
                    break;
                }
            }
            SaveToFile();
        }

        /// <summary>
        /// Sla alle aanvragen op naar het JSON-bestand.
        /// </summary>
        private static void SaveToFile()
        {
            string json = JsonConvert.SerializeObject(aanvragen, Formatting.Indented);

            // Controleer of de map bestaat, zo niet: maak aan
            string directory = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(FilePath, json);
        }

        /// <summary>
        /// Laad alle aanvragen vanuit het JSON-bestand in het geheugen.
        /// </summary>
        private static void LoadFromFile()
        {
            if (File.Exists(FilePath))
            {
                string json = File.ReadAllText(FilePath);
                List<RegistratieAanvraag> loaded = JsonConvert.DeserializeObject<List<RegistratieAanvraag>>(json);
                if (loaded != null)
                {
                    aanvragen = loaded;
                }
                else
                {
                    aanvragen = new List<RegistratieAanvraag>();
                }

                // Zet het volgende ID correct
                if (aanvragen.Count > 0)
                {
                    volgendeId = aanvragen[aanvragen.Count - 1].Id + 1;
                }
                else
                {
                    volgendeId = 1;
                }
            }
        }
    }
}
