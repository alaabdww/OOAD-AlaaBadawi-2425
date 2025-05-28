using System.Data.SqlClient;
using System.Configuration;
using BenchmarkToolLibrary.Helpers; // Voor HashPassword

public static class UserData
{
    private static string connString = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString;

    /// <summary>
    /// Valideert login/wachtwoord. Retourneert bedrijfId bij succes, anders 0.
    /// </summary>
    public static int ValidateLogin(string login, string plainWachtwoord)
    {
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
                        string hashInput = PasswordHelper.HashPassword(plainWachtwoord);
                        if (hashDb == hashInput)
                            return id;
                    }
                }
            }
        }
        return 0;
    }
}
