using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using BenchmarkToolLibrary.Models;

namespace BenchmarkToolLibrary.Data
{
    /// <summary>
    /// Datalaag voor jaarrapporten en benchmarking (volgens ERD).
    /// </summary>
    public static class RapportData
    {
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
                            // Je ERD heeft geen datum in Yearreports, enkel 'year', 'fte', 'company_id'
                            // Eventueel uitbreiden als je meer kolommen wil tonen

                            // Modelklasse Jaarrapport moet een constructor hebben met: (int id, int bedrijfId, int jaar, DateTime rapportdatum, string status)
                            // Omdat je geen rapportdatum/status hebt in de tabel, vul aan met default
                            Jaarrapport rapport = new Jaarrapport(id, bedrijfId, jaar, DateTime.Now, "concept");
                            lijst.Add(rapport);
                        }
                    }
                }
            }
            return lijst;
        }

        /// <summary>
        /// Haal alle jaren op waarvoor het bedrijf een rapport heeft.
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
                        {
                            jaren.Add((int)reader["year"]);
                        }
                    }
                }
            }
            return jaren;
        }

        /// <summary>
        /// Haal benchmarkinggegevens op: naam bedrijf + totale kosten voor een bepaald jaar.
        /// </summary>
        public static List<BenchmarkResultaat> GetVergelijking(int jaar)
        {
            List<BenchmarkResultaat> resultaten = new List<BenchmarkResultaat>();
            string query = @"
                SELECT c.name AS NaamBedrijf, 
                       SUM(k.value) AS Waarde
                FROM Yearreports y
                INNER JOIN Companies c ON y.company_id = c.id
                INNER JOIN Costs k ON y.id = k.yearreport_id
                WHERE y.year = @jaar
                GROUP BY c.name
                ORDER BY Waarde DESC";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@jaar", jaar);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string naam = reader["NaamBedrijf"] as string ?? "";
                            double waarde = reader["Waarde"] != DBNull.Value ? Convert.ToDouble(reader["Waarde"]) : 0.0;
                            BenchmarkResultaat res = new BenchmarkResultaat();
                            res.NaamBedrijf = naam;
                            res.Waarde = waarde;
                            resultaten.Add(res);
                        }
                    }
                }
            }
            return resultaten;
        }

        /// <summary>
        /// Verwijdert een jaarrapport.
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
