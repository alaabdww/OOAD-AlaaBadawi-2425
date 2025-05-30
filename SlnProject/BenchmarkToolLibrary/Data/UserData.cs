// UserData.cs
// -------------
// Deze klasse beheert authenticatie (login/wachtwoord) van gebruikers tegen de Companies-tabel in de SQL-database.
// Gebruikt een hashfunctie voor wachtwoorden en retourneert het bedrijfId als de login/wachtwoord-combinatie correct is.

using System.Data.SqlClient;
using System.Configuration;
using BenchmarkToolLibrary.Helpers; // Voor HashPassword

public static class UserData
{
    // Connection string naar de SQL database uit de configuratie
    private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

    /// <summary>
    /// Valideert de login/wachtwoord-combinatie van de gebruiker.
    /// Retourneert het bedrijfId bij succesvolle login, anders 0.
    /// </summary>
    /// <param name="login">De gebruikersnaam/login</param>
    /// <param name="plainWachtwoord">Het wachtwoord in platte tekst</param>
    /// <returns>BedrijfId bij succes, anders 0</returns>
    public static int ValidateLogin(string login, string plainWachtwoord)
    {
        // Zoek de gebruiker met de opgegeven login op en haal het wachtwoordhash op
        string query = "SELECT id, password FROM Companies WHERE login = @login";
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
                        int id = (int)reader["id"];
                        string hashDb = reader["password"] as string ?? "";
                        // Hash het ingevoerde wachtwoord om te vergelijken met de opgeslagen hash
                        string hashInput = PasswordHelper.HashPassword(plainWachtwoord);
                        if (hashDb == hashInput)
                            return id; // Login correct, return bedrijfId
                    }
                }
            }
        }
        return 0; // Login/wachtwoord fout
    }
}
