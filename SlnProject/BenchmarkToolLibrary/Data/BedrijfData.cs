using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using BenchmarkToolLibrary.Models;
using BenchmarkToolLibrary.Helpers;

namespace BenchmarkToolLibrary.Data
{
    /// <summary>
    /// Bevat alle database-acties voor de Company/Bedrijf-tabel.
    /// </summary>
    public class BedrijfData
    {
        private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

        /// <summary>
        /// Haalt alle bedrijven uit de database op.
        /// Het wachtwoordveld van elk bedrijf is altijd leeg (de hash wordt niet geladen).
        /// </summary>
        /// <returns>Lijst van bedrijven</returns>
        public static List<Bedrijf> GetAll()
        {
            List<Bedrijf> bedrijven = new List<Bedrijf>();
            string query = @"SELECT id, name, contact, address, zip, city, country, phone, email, btw, login, regdate, acceptdate, lastmodified, status, language, logo, nacecode_code FROM Companies";
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
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
                                string wachtwoord = ""; // Nooit de hash teruggeven!
                                DateTime registratiedatum = reader["regdate"] != DBNull.Value ? (DateTime)reader["regdate"] : DateTime.MinValue;
                                DateTime? acceptatiedatum = reader["acceptdate"] != DBNull.Value ? (DateTime?)reader["acceptdate"] : null;
                                DateTime laatstGewijzigd = reader["lastmodified"] != DBNull.Value ? (DateTime)reader["lastmodified"] : DateTime.MinValue;
                                string status = reader["status"] as string ?? "nieuw";
                                string taal = reader["language"] as string ?? "";
                                byte[] logo = reader["logo"] != DBNull.Value ? (byte[])reader["logo"] : null;
                                string nacecode = reader["nacecode_code"] as string ?? "";

                                Bedrijf bedrijf = new Bedrijf(
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fout bij ophalen bedrijven: " + ex.Message);
            }
            return bedrijven;
        }

        /// <summary>
        /// Voegt een nieuw bedrijf toe aan de database.
        /// Het wachtwoord wordt automatisch gehasht met PasswordHelper.
        /// </summary>
        /// <param name="bedrijf">Bedrijf met cleartext wachtwoord</param>
        public static void Insert(Bedrijf bedrijf)
        {
            string query = @"INSERT INTO Companies 
                                (name, contact, address, zip, city, country, phone, email, btw, login, password, regdate, acceptdate, lastmodified, status, language, logo, nacecode_code) 
                             VALUES 
                                (@naam, @contact, @adres, @postcode, @gemeente, @land, @telefoon, @email, @btwNummer, @login, @password, @registratiedatum, @acceptatiedatum, @laatstGewijzigd, @status, @taal, @logo, @nacecode)";
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@naam", bedrijf.Naam);
                        command.Parameters.AddWithValue("@contact", bedrijf.Contactpersoon);
                        command.Parameters.AddWithValue("@adres", bedrijf.Adres);
                        command.Parameters.AddWithValue("@postcode", bedrijf.Postcode);
                        command.Parameters.AddWithValue("@gemeente", bedrijf.Gemeente);
                        command.Parameters.AddWithValue("@land", bedrijf.Land);
                        command.Parameters.AddWithValue("@telefoon", bedrijf.Telefoon);
                        command.Parameters.AddWithValue("@email", bedrijf.Email);
                        command.Parameters.AddWithValue("@btwNummer", bedrijf.BtwNummer);
                        command.Parameters.AddWithValue("@login", bedrijf.Login);

                        string wachtwoordHash = PasswordHelper.HashPassword(bedrijf.Wachtwoord ?? "");
                        command.Parameters.AddWithValue("@password", wachtwoordHash);

                        command.Parameters.AddWithValue("@registratiedatum", bedrijf.Registratiedatum);
                        if (bedrijf.Acceptatiedatum.HasValue)
                            command.Parameters.AddWithValue("@acceptatiedatum", bedrijf.Acceptatiedatum.Value);
                        else
                            command.Parameters.AddWithValue("@acceptatiedatum", DBNull.Value);
                        command.Parameters.AddWithValue("@laatstGewijzigd", bedrijf.LaatstGewijzigd);
                        command.Parameters.AddWithValue("@status", bedrijf.Status);
                        command.Parameters.AddWithValue("@taal", bedrijf.Taal);
                        command.Parameters.AddWithValue("@logo", bedrijf.Logo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@nacecode", bedrijf.Nacecode);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fout bij toevoegen bedrijf: " + ex.Message);
            }
        }

        /// <summary>
        /// Wijzigt alle velden van een bestaand bedrijf, behalve het wachtwoord.
        /// Geef een nieuwe waarde voor nieuwWachtwoord als het wachtwoord moet worden aangepast.
        /// </summary>
        /// <param name="bedrijf">Bedrijf (wachtwoordveld wordt genegeerd)</param>
        /// <param name="nieuwWachtwoord">Indien niet leeg: nieuw wachtwoord (cleartext), anders wordt het wachtwoord niet aangepast</param>
        public static void Update(Bedrijf bedrijf, string nieuwWachtwoord)
        {
            string query;
            bool wachtwoordWijzigen = !string.IsNullOrEmpty(nieuwWachtwoord);

            if (wachtwoordWijzigen)
            {
                query = @"UPDATE Companies SET 
                                name = @naam, contact = @contact, address = @adres, zip = @postcode, city = @gemeente, country = @land, 
                                phone = @telefoon, email = @email, btw = @btwNummer, login = @login, password = @password, 
                                regdate = @registratiedatum, acceptdate = @acceptatiedatum, lastmodified = @laatstGewijzigd, 
                                status = @status, language = @taal, logo = @logo, nacecode_code = @nacecode
                             WHERE id = @id";
            }
            else
            {
                query = @"UPDATE Companies SET 
                                name = @naam, contact = @contact, address = @adres, zip = @postcode, city = @gemeente, country = @land, 
                                phone = @telefoon, email = @email, btw = @btwNummer, login = @login, 
                                regdate = @registratiedatum, acceptdate = @acceptatiedatum, lastmodified = @laatstGewijzigd, 
                                status = @status, language = @taal, logo = @logo, nacecode_code = @nacecode
                             WHERE id = @id";
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@naam", bedrijf.Naam);
                        command.Parameters.AddWithValue("@contact", bedrijf.Contactpersoon);
                        command.Parameters.AddWithValue("@adres", bedrijf.Adres);
                        command.Parameters.AddWithValue("@postcode", bedrijf.Postcode);
                        command.Parameters.AddWithValue("@gemeente", bedrijf.Gemeente);
                        command.Parameters.AddWithValue("@land", bedrijf.Land);
                        command.Parameters.AddWithValue("@telefoon", bedrijf.Telefoon);
                        command.Parameters.AddWithValue("@email", bedrijf.Email);
                        command.Parameters.AddWithValue("@btwNummer", bedrijf.BtwNummer);
                        command.Parameters.AddWithValue("@login", bedrijf.Login);

                        if (wachtwoordWijzigen)
                        {
                            string wachtwoordHash = PasswordHelper.HashPassword(nieuwWachtwoord);
                            command.Parameters.AddWithValue("@password", wachtwoordHash);
                        }

                        command.Parameters.AddWithValue("@registratiedatum", bedrijf.Registratiedatum);
                        if (bedrijf.Acceptatiedatum.HasValue)
                            command.Parameters.AddWithValue("@acceptatiedatum", bedrijf.Acceptatiedatum.Value);
                        else
                            command.Parameters.AddWithValue("@acceptatiedatum", DBNull.Value);
                        command.Parameters.AddWithValue("@laatstGewijzigd", bedrijf.LaatstGewijzigd);
                        command.Parameters.AddWithValue("@status", bedrijf.Status);
                        command.Parameters.AddWithValue("@taal", bedrijf.Taal);
                        command.Parameters.AddWithValue("@logo", bedrijf.Logo ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@nacecode", bedrijf.Nacecode);
                        command.Parameters.AddWithValue("@id", bedrijf.Id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fout bij wijzigen bedrijf: " + ex.Message);
            }
        }

        /// <summary>
        /// Wijzigt alleen het wachtwoord van het opgegeven bedrijf.
        /// </summary>
        /// <param name="bedrijfId">Het ID van het bedrijf</param>
        /// <param name="nieuwWachtwoord">Het nieuwe cleartext wachtwoord</param>
        public static void UpdateWachtwoord(int bedrijfId, string nieuwWachtwoord)
        {
            string query = "UPDATE Companies SET password = @password WHERE id = @id";
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        string wachtwoordHash = PasswordHelper.HashPassword(nieuwWachtwoord ?? "");
                        command.Parameters.AddWithValue("@password", wachtwoordHash);
                        command.Parameters.AddWithValue("@id", bedrijfId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fout bij wachtwoord wijzigen: " + ex.Message);
            }
        }

        /// <summary>
        /// Leest het logo van een bedrijf uit als byte array.
        /// </summary>
        /// <param name="bedrijfId">Bedrijfs-ID</param>
        /// <returns>Logo als byte[] of null</returns>
        public static byte[] GetLogo(int bedrijfId)
        {
            string query = "SELECT logo FROM Companies WHERE id = @id";
            try
            {
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fout bij ophalen logo: " + ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Wijzigt alleen het logo van een bedrijf.
        /// </summary>
        /// <param name="bedrijfId">Bedrijfs-ID</param>
        /// <param name="logo">Logo (byte[])</param>
        public static void UpdateLogo(int bedrijfId, byte[] logo)
        {
            string query = "UPDATE Companies SET logo = @logo WHERE id = @id";
            try
            {
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
            catch (Exception ex)
            {
                Console.WriteLine("Fout bij wijzigen logo: " + ex.Message);
            }
        }

        /// <summary>
        /// Authenticeert een gebruiker via login en wachtwoord (cleartext).
        /// </summary>
        /// <param name="login">Loginnaam</param>
        /// <param name="password">Wachtwoord (cleartext)</param>
        /// <returns>True als login en wachtwoord correct zijn</returns>
        public static bool Authenticate(string login, string password)
        {
            string query = "SELECT password FROM Companies WHERE login = @login";
            try
            {
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@login", login);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hashFromDb = reader["password"] as string;
                                return PasswordHelper.VerifyPassword(password, hashFromDb);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fout bij authenticatie: " + ex.Message);
            }
            return false;
        }
    }
}
