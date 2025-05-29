using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Helpers;

namespace BenchmarkToolLibrary.Data
{
    public class BedrijfData
    {
        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        public static List<Bedrijf> GetAll()
        {
            List<Bedrijf> bedrijven = new List<Bedrijf>();
            string query = @"SELECT id, name, contact, address, zip, city, country, phone, email, btw, login, regdate, acceptdate, lastmodified, status, language, logo, nacecode_code FROM Companies";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bedrijven.Add(FromReader(reader));
                        }
                    }
                }
            }
            return bedrijven;
        }

        public static void Insert(Bedrijf bedrijf)
        {
            // Bepaal eerstvolgende vrije id:
            int newId = GetNextId();

            string query = @"INSERT INTO Companies 
        (id, name, contact, address, zip, city, country, phone, email, btw, login, password, regdate, acceptdate, lastmodified, status, language, logo, nacecode_code) 
        VALUES 
        (@id, @naam, @contact, @adres, @postcode, @gemeente, @land, @telefoon, @email, @btwNummer, @login, @password, @registratiedatum, @acceptatiedatum, @laatstGewijzigd, @status, @taal, @logo, @nacecode)";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", newId);
                    cmd.Parameters.AddWithValue("@naam", bedrijf.Naam ?? "");
                    cmd.Parameters.AddWithValue("@contact", bedrijf.Contactpersoon ?? "");
                    cmd.Parameters.AddWithValue("@adres", bedrijf.Adres ?? "");
                    cmd.Parameters.AddWithValue("@postcode", bedrijf.Postcode ?? "");
                    cmd.Parameters.AddWithValue("@gemeente", bedrijf.Gemeente ?? "");
                    cmd.Parameters.AddWithValue("@land", bedrijf.Land ?? "");
                    cmd.Parameters.AddWithValue("@telefoon", bedrijf.Telefoon ?? "");
                    cmd.Parameters.AddWithValue("@email", bedrijf.Email ?? "");
                    cmd.Parameters.AddWithValue("@btwNummer", bedrijf.BtwNummer ?? "");
                    cmd.Parameters.AddWithValue("@login", bedrijf.Login ?? "");
                    string wachtwoordHash = PasswordHelper.HashPassword(bedrijf.Wachtwoord ?? "");
                    cmd.Parameters.AddWithValue("@password", wachtwoordHash);
                    cmd.Parameters.AddWithValue("@registratiedatum", bedrijf.Registratiedatum);
                    if (bedrijf.Acceptatiedatum.HasValue)
                        cmd.Parameters.AddWithValue("@acceptatiedatum", bedrijf.Acceptatiedatum.Value);
                    else
                        cmd.Parameters.AddWithValue("@acceptatiedatum", DBNull.Value);
                    cmd.Parameters.AddWithValue("@laatstGewijzigd", bedrijf.LaatstGewijzigd);
                    cmd.Parameters.AddWithValue("@status", bedrijf.Status ?? "nieuw");
                    cmd.Parameters.AddWithValue("@taal", bedrijf.Taal ?? "");
                    cmd.Parameters.AddWithValue("@logo", bedrijf.Logo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nacecode", bedrijf.Nacecode ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static void Update(Bedrijf bedrijf, string nieuwWachtwoord)
        {
            bool wachtwoordWijzigen = !string.IsNullOrEmpty(nieuwWachtwoord);
            string query = wachtwoordWijzigen ?
                @"UPDATE Companies SET name=@naam, contact=@contact, address=@adres, zip=@postcode, city=@gemeente, country=@land, phone=@telefoon, email=@email, btw=@btwNummer, login=@login, password=@password, regdate=@registratiedatum, acceptdate=@acceptatiedatum, lastmodified=@laatstGewijzigd, status=@status, language=@taal, logo=@logo, nacecode_code=@nacecode WHERE id=@id"
                :
                @"UPDATE Companies SET name=@naam, contact=@contact, address=@adres, zip=@postcode, city=@gemeente, country=@land, phone=@telefoon, email=@email, btw=@btwNummer, login=@login, regdate=@registratiedatum, acceptdate=@acceptatiedatum, lastmodified=@laatstGewijzigd, status=@status, language=@taal, logo=@logo, nacecode_code=@nacecode WHERE id=@id";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@naam", bedrijf.Naam ?? "");
                    cmd.Parameters.AddWithValue("@contact", bedrijf.Contactpersoon ?? "");
                    cmd.Parameters.AddWithValue("@adres", bedrijf.Adres ?? "");
                    cmd.Parameters.AddWithValue("@postcode", bedrijf.Postcode ?? "");
                    cmd.Parameters.AddWithValue("@gemeente", bedrijf.Gemeente ?? "");
                    cmd.Parameters.AddWithValue("@land", bedrijf.Land ?? "");
                    cmd.Parameters.AddWithValue("@telefoon", bedrijf.Telefoon ?? "");
                    cmd.Parameters.AddWithValue("@email", bedrijf.Email ?? "");
                    cmd.Parameters.AddWithValue("@btwNummer", bedrijf.BtwNummer ?? "");
                    cmd.Parameters.AddWithValue("@login", bedrijf.Login ?? "");
                    if (wachtwoordWijzigen)
                        cmd.Parameters.AddWithValue("@password", PasswordHelper.HashPassword(nieuwWachtwoord));
                    cmd.Parameters.AddWithValue("@registratiedatum", bedrijf.Registratiedatum);
                    if (bedrijf.Acceptatiedatum.HasValue)
                        cmd.Parameters.AddWithValue("@acceptatiedatum", bedrijf.Acceptatiedatum.Value);
                    else
                        cmd.Parameters.AddWithValue("@acceptatiedatum", DBNull.Value);
                    cmd.Parameters.AddWithValue("@laatstGewijzigd", bedrijf.LaatstGewijzigd);
                    cmd.Parameters.AddWithValue("@status", bedrijf.Status ?? "nieuw");
                    cmd.Parameters.AddWithValue("@taal", bedrijf.Taal ?? "");
                    cmd.Parameters.AddWithValue("@logo", bedrijf.Logo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@nacecode", bedrijf.Nacecode ?? "");
                    cmd.Parameters.AddWithValue("@id", bedrijf.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateStatus(int bedrijfId, string status)
        {
            string query = "UPDATE Companies SET status = @status WHERE id = @id";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@id", bedrijfId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static int GetNextId()
        {
            int nextId = 1;
            string query = "SELECT MAX(id) FROM Companies";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                        nextId = (int)result + 1;
                }
            }
            return nextId;
        }


        public static Bedrijf GetById(int bedrijfId)
        {
            Bedrijf bedrijf = null;
            string query = "SELECT id, name, contact, address, zip, city, country, phone, email, btw, login, regdate, acceptdate, lastmodified, status, language, logo, nacecode_code FROM Companies WHERE id = @id";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", bedrijfId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            bedrijf = FromReader(reader);
                    }
                }
            }
            return bedrijf;
        }

        public static List<Bedrijf> GetByStatus(string status)
        {
            List<Bedrijf> bedrijven = new List<Bedrijf>();
            string query = @"SELECT id, name, contact, address, zip, city, country, phone, email, btw, login, regdate, acceptdate, lastmodified, status, language, logo, nacecode_code FROM Companies WHERE status = @status";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            bedrijven.Add(FromReader(reader));
                    }
                }
            }
            return bedrijven;
        }

        public static void Delete(int bedrijfId)
        {
            string query = "DELETE FROM Companies WHERE id = @id";
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", bedrijfId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static Bedrijf FromReader(SqlDataReader reader)
        {
            return new Bedrijf(
                (int)reader["id"],
                reader["name"] as string ?? "",
                reader["contact"] as string ?? "",
                reader["address"] as string ?? "",
                reader["zip"] as string ?? "",
                reader["city"] as string ?? "",
                reader["country"] as string ?? "",
                reader["phone"] as string ?? "",
                reader["email"] as string ?? "",
                reader["btw"] as string ?? "",
                reader["login"] as string ?? "",
                "", // wachtwoord NOOIT uitlezen
                reader["regdate"] != DBNull.Value ? (DateTime)reader["regdate"] : DateTime.MinValue,
                reader["acceptdate"] != DBNull.Value ? (DateTime?)reader["acceptdate"] : null,
                reader["lastmodified"] != DBNull.Value ? (DateTime)reader["lastmodified"] : DateTime.MinValue,
                reader["status"] as string ?? "nieuw",
                reader["language"] as string ?? "",
                reader["logo"] != DBNull.Value ? (byte[])reader["logo"] : null,
                reader["nacecode_code"] as string ?? ""
            );
        }
    }
}
