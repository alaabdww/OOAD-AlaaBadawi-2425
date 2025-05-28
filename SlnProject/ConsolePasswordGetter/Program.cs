using BenchmarkToolLibrary.Helpers;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        string connStr = "Server=(localdb)\\MSSQLLocalDB;Database=BenchmarkDB;Trusted_Connection=True;"; // pas aan indien nodig

        Console.WriteLine(PasswordHelper.HashPassword("test7"));

        Console.Write("Geef de login in: ");
        string login = Console.ReadLine();

        using (SqlConnection con = new SqlConnection(connStr))
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT id FROM Companies WHERE login = @login", con);
            cmd.Parameters.AddWithValue("@login", login);

            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                int id = (int)result;
                string password = "test" + id;
                string hash = GetHash(password);
                Console.WriteLine("Login: " + login);
                Console.WriteLine("Wachtwoord (plain): " + password);
                Console.WriteLine("Wachtwoord-hash (SHA256): " + hash);
            }
            else
            {
                Console.WriteLine("Login niet gevonden.");
            }
        }

        Console.WriteLine("Druk op een toets om af te sluiten...");
        Console.ReadKey();
    }

    static string GetHash(string input)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha.ComputeHash(bytes);
            StringBuilder builder = new StringBuilder();
            foreach (byte b in hashBytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }
}
