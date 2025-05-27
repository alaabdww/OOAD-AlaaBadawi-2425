using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using BenchmarkToolLibrary.Models;

namespace BenchmarkToolLibrary.Data
{
    public class BedrijfData
    {
        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public static void UpdateLogo(int bedrijfId, byte[] logo)
        {
            string query = "UPDATE Companies SET logo = @logo WHERE id = @id";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@logo", logo ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@id", bedrijfId);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static byte[] GetLogo(int bedrijfId)
        {
            string query = "SELECT logo FROM Companies WHERE id = @id";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", bedrijfId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            return (byte[])reader["logo"];
                        }
                    }
                }
            }
            return null;
        }

    }
}
