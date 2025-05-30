// RapportData.cs
// --------------
// Deze klasse bevat de data-access-methodes om jaarrapporten en benchmarkgegevens op te halen en te beheren
// voor bedrijven. Er wordt gewerkt met SQL Server en enkel deze klasse praat direct met de database
// voor rapport-gerelateerde data (jaarrapporten en benchmarking).

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using BenchmarkToolLibrary.Models;

namespace BenchmarkToolLibrary.Data
{
    public static class RapportData
    {
        // Connection string voor de SQL-database
        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        /// <summary>
        /// Haal alle jaarrapporten op voor een specifiek bedrijf.
        /// </summary>
        public static List<Jaarrapport> GetAllVoorBedrijf(int bedrijfId)
        {
            List<Jaarrapport> lijst = new List<Jaarrapport>();
            string query = "SELECT id, year, company_id FROM Yearreports WHERE company_id = @bedrijfid";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bedrijfid", bedrijfId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["id"];
                            int jaar = (int)reader["year"];
                            int compId = (int)reader["company_id"];
                            // NB: rapportdatum en status zijn niet aanwezig in tabel, defaults worden gebruikt.
                            DateTime rapportdatum = DateTime.Now;
                            string status = "concept";
                            lijst.Add(new Jaarrapport(id, compId, jaar, rapportdatum, status));
                        }
                    }
                }
            }
            return lijst;
        }

        /// <summary>
        /// Geeft een lijst van alle jaren waarvoor dit bedrijf een rapport heeft.
        /// </summary>
        public static List<int> GetJarenVoorBedrijf(int bedrijfId)
        {
            List<int> jaren = new List<int>();
            string query = "SELECT DISTINCT year FROM Yearreports WHERE company_id = @bedrijfid ORDER BY year";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bedrijfid", bedrijfId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                            jaren.Add((int)reader["year"]);
                    }
                }
            }
            return jaren;
        }

        /// <summary>
        /// Haal benchmarkinggegevens op (totaal per categorie voor 1 jaar en bedrijf).
        /// </summary>
        public static List<BenchmarkResultaat> GetVergelijking(int bedrijfId, int jaar)
        {
            var resultaten = new List<BenchmarkResultaat>();

            string query = @"
                SELECT c.text AS CategorieNaam, SUM(k.value) AS Waarde
                FROM Yearreports y
                INNER JOIN Costs k ON y.id = k.yearreport_id
                INNER JOIN Categories c ON k.category_nr = c.nr
                WHERE y.company_id = @bedrijfId AND y.year = @jaar
                GROUP BY c.text
                ORDER BY Waarde DESC";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bedrijfId", bedrijfId);
                    command.Parameters.AddWithValue("@jaar", jaar);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultaten.Add(new BenchmarkResultaat
                            {
                                CategorieNaam = reader["CategorieNaam"] as string ?? "",
                                Waarde = reader["Waarde"] != DBNull.Value ? Convert.ToDouble(reader["Waarde"]) : 0.0
                            });
                        }
                    }
                }
            }
            return resultaten;
        }

        /// <summary>
        /// Verwijdert een jaarrapport uit de database.
        /// </summary>
        public static void Delete(int jaarrapportId)
        {
            string query = "DELETE FROM Yearreports WHERE id = @id";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", jaarrapportId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
