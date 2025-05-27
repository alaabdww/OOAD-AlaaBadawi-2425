using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkToolLibrary.Models;
using System.Data.SqlClient;
using System.Configuration;


namespace BenchmarkToolLibrary.Data
{
    public class BedrijfData
    {
        // Connection string wordt centraal opgehaald uit App.config
        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public static List<Bedrijf> GetAll()
        {
            var bedrijven = new List<Bedrijf>();
            string query = @"SELECT id, name, contact, address, zip, city, country, phone, email, btw, login, password, regdate, acceptdate, lastmodified, status, language, logo, nacecode_code FROM Companies";

            using (var connection = new SqlConnection(connString))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["id"];
                            string naam = reader["name"] as string ?? "";
                            string contactpersoon = reader["contact"] as string ?? "";
                            string adres = reader["address"] as string ?? "";
                            string postcode = reader["zip"] as string ?? "";
                            string gemeente = reader["city"] as string ?? "";
                            string land = reader["country"] as string ?? "";
                            string telefoon = reader["phone"] as string ?? "";
                            string email = reader["email"] as string ?? "";
                            string btwNummer = reader["btw"] as string ?? "";
                            string login = reader["login"] as string ?? "";
                            string wachtwoord = reader["password"] as string ?? "";
                            DateTime registratiedatum = reader["regdate"] != DBNull.Value ? (DateTime)reader["regdate"] : DateTime.MinValue;
                            DateTime? acceptatiedatum = reader["acceptdate"] != DBNull.Value ? (DateTime?)reader["acceptdate"] : null;
                            DateTime laatstGewijzigd = reader["lastmodified"] != DBNull.Value ? (DateTime)reader["lastmodified"] : DateTime.MinValue;
                            string status = reader["status"] as string ?? "nieuw";
                            string taal = reader["language"] as string ?? "";
                            string logo = reader["logo"] as string ?? "";
                            string nacecode = reader["nacecode_code"] as string ?? "";

                            var bedrijf = new Bedrijf(
                                id,
                                naam,
                                contactpersoon,
                                adres,
                                postcode,
                                gemeente,
                                land,
                                telefoon,
                                email,
                                btwNummer,
                                login,
                                wachtwoord,
                                registratiedatum,
                                acceptatiedatum,
                                laatstGewijzigd,
                                status,
                                taal,
                                logo,
                                nacecode
                            );

                            bedrijven.Add(bedrijf);
                        }
                    }
                }
            }
            return bedrijven;
        }
    }
}
