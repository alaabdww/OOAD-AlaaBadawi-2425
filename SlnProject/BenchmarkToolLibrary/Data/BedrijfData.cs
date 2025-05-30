// BedrijfData.cs
// ---------------------
// Deze class verzorgt alle database-operaties voor bedrijven in het BenchmarkTool-project.
// Je kan hiermee bedrijven ophalen, toevoegen, wijzigen, verwijderen, en het logo standaardiseren.
// Werkt altijd met een fallback-logo indien geen afbeelding aanwezig is.
// SQL-interactie gebeurt altijd veilig met parameters en wachtwoorden worden gehasht opgeslagen.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Helpers;
using System.Reflection;
using System.IO;

namespace BenchmarkToolLibrary.Data
{
    public class BedrijfData
    {
        // Connectionstring uit App.config
        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        // Standaard (lege) logo-afbeelding als fallback voor bedrijven zonder logo.
        private static byte[] _emptyLogo;

        // Static constructor: laad lege logo bij het opstarten van de applicatie.
        static BedrijfData()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("BenchmarkToolLibrary.Resources.empty.png");
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            _emptyLogo = ms.ToArray();
        }

        /// <summary>
        /// Haalt alle bedrijven uit de database (Companies-tabel).
        /// </summary>
        public static List<Bedrijf> GetAll()
        {
            List<Bedrijf> bedrijven = new List<Bedrijf>();
            string query = @"SELECT id, name, contact, address, zip, city, country, phone, email, btw, login, password, regdate, acceptdate, lastmodified, status, language, logo, nacecode_code FROM Companies";
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

        /// <summary>
        /// Voegt een nieuw bedrijf toe aan de database.
        /// </summary>
        public static void Insert(Bedrijf bedrijf)
        {
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
                    cmd.Parameters.AddWithValue("@email", bedrijf.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@btwNummer", bedrijf.BtwNummer ?? "");
                    cmd.Parameters.AddWithValue("@login", bedrijf.Login ?? "");
                    string wachtwoordHash = PasswordHelper.HashPassword(bedrijf.Wachtwoord ?? "");
                    cmd.Parameters.AddWithValue("@password", wachtwoordHash);
                    cmd.Parameters.AddWithValue("@registratiedatum", bedrijf.Registratiedatum);
                    cmd.Parameters.AddWithValue("@acceptatiedatum", bedrijf.Acceptatiedatum.HasValue ? (object)bedrijf.Acceptatiedatum.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@laatstGewijzigd", bedrijf.LaatstGewijzigd);
                    cmd.Parameters.AddWithValue("@status", bedrijf.Status ?? "nieuw");
                    cmd.Parameters.AddWithValue("@taal", bedrijf.Taal ?? "");

                    // Gebruik standaardafbeelding indien nodig
                    byte[] logoToSave = bedrijf.Logo;
                    if (logoToSave == null || logoToSave.Length == 0)
                        logoToSave = _emptyLogo;
                    cmd.Parameters.AddWithValue("@logo", logoToSave);

                    cmd.Parameters.AddWithValue("@nacecode", bedrijf.Nacecode ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Wijzigt een bestaand bedrijf. Optioneel ook wachtwoord aanpassen.
        /// </summary>
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
                    cmd.Parameters.AddWithValue("@email", bedrijf.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@btwNummer", bedrijf.BtwNummer ?? "");
                    cmd.Parameters.AddWithValue("@login", bedrijf.Login ?? "");
                    if (wachtwoordWijzigen)
                        cmd.Parameters.AddWithValue("@password", PasswordHelper.HashPassword(nieuwWachtwoord));
                    cmd.Parameters.AddWithValue("@registratiedatum", bedrijf.Registratiedatum);
                    cmd.Parameters.AddWithValue("@acceptatiedatum", bedrijf.Acceptatiedatum.HasValue ? (object)bedrijf.Acceptatiedatum.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@laatstGewijzigd", bedrijf.LaatstGewijzigd);
                    cmd.Parameters.AddWithValue("@status", bedrijf.Status ?? "nieuw");
                    cmd.Parameters.AddWithValue("@taal", bedrijf.Taal ?? "");

                    // Gebruik standaardafbeelding indien nodig
                    byte[] logoToSave = bedrijf.Logo;
                    if (logoToSave == null || logoToSave.Length == 0)
                        logoToSave = _emptyLogo;
                    cmd.Parameters.AddWithValue("@logo", logoToSave);

                    cmd.Parameters.AddWithValue("@nacecode", bedrijf.Nacecode ?? "");
                    cmd.Parameters.AddWithValue("@id", bedrijf.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update enkel de status van een bedrijf op basis van het id.
        /// </summary>
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

        /// <summary>
        /// Haalt het volgende beschikbare id op voor een nieuw bedrijf.
        /// </summary>
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
                        nextId = Convert.ToInt32(result) + 1;
                }
            }
            return nextId;
        }

        /// <summary>
        /// Haalt een bedrijf op basis van het id.
        /// </summary>
        public static Bedrijf GetById(int bedrijfId)
        {
            Bedrijf bedrijf = null;
            string query = "SELECT id, name, contact, address, zip, city, country, phone, email, btw, login, password, regdate, acceptdate, lastmodified, status, language, logo, nacecode_code FROM Companies WHERE id = @id";
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

        /// <summary>
        /// Haalt alle bedrijven op met een bepaalde status.
        /// </summary>
        public static List<Bedrijf> GetByStatus(string status)
        {
            List<Bedrijf> bedrijven = new List<Bedrijf>();
            string query = @"SELECT id, name, contact, address, zip, city, country, phone, email, btw, login, password, regdate, acceptdate, lastmodified, status, language, logo, nacecode_code FROM Companies WHERE status = @status";
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

        /// <summary>
        /// Verwijdert een bedrijf uit de database.
        /// </summary>
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

        /// <summary>
        /// Maakt van een SqlDataReader een Bedrijf-object.
        /// </summary>
        private static Bedrijf FromReader(SqlDataReader reader)
        {
            string nacecodeString = "";
            if (reader["nacecode_code"] != DBNull.Value)
            {
                if (reader["nacecode_code"] is byte[])
                    nacecodeString = System.Text.Encoding.UTF8.GetString((byte[])reader["nacecode_code"]);
                else if (reader["nacecode_code"] is string)
                    nacecodeString = (string)reader["nacecode_code"];
            }

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
                "", // wachtwoord NOOIT uitlezen!
                reader["regdate"] != DBNull.Value ? (DateTime)reader["regdate"] : DateTime.MinValue,
                reader["acceptdate"] != DBNull.Value ? (DateTime?)reader["acceptdate"] : null,
                reader["lastmodified"] != DBNull.Value ? (DateTime)reader["lastmodified"] : DateTime.MinValue,
                reader["status"] as string ?? "nieuw",
                reader["language"] as string ?? "",
                reader["logo"] != DBNull.Value ? (byte[])reader["logo"] : null,
                nacecodeString
            );
        }
    }
}
