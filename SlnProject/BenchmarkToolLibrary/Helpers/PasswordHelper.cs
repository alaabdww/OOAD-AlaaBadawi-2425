// PasswordHelper.cs
// -----------------
// Deze helperklasse bevat statische methodes voor veilige wachtwoordafhandeling: 
// het genereren van een SHA256-hash (in Base64) en het vergelijken van wachtwoorden.
// Gebruikt in authenticatie & opslag van wachtwoorden in de database.

using System;
using System.Security.Cryptography;
using System.Text;

namespace BenchmarkToolLibrary.Helpers
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Maakt een SHA256-hash van het opgegeven wachtwoord en geeft dit terug als een Base64-string.
        /// Dit formaat wordt gebruikt voor opslag in de database.
        /// </summary>
        /// <param name="password">Het wachtwoord in platte tekst</param>
        /// <returns>SHA256-hash als Base64-string</returns>
        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                // Zet het wachtwoord om naar bytes
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                // Maak de SHA256-hash
                byte[] hash = sha.ComputeHash(bytes);
                // Zet de hash om naar een Base64-string voor opslag in de database
                return Convert.ToBase64String(hash);
            }
        }

        /// <summary>
        /// Vergelijkt een ingevoerd wachtwoord met een opgeslagen hash.
        /// </summary>
        /// <param name="password">Het ingevoerde wachtwoord in platte tekst</param>
        /// <param name="hash">De opgeslagen hash (Base64)</param>
        /// <returns>True als het wachtwoord klopt, anders false</returns>
        public static bool VerifyPassword(string password, string hash)
        {
            // Hash het ingevoerde wachtwoord en vergelijk met opgeslagen hash
            string hashInput = HashPassword(password);
            return hashInput == hash;
        }
    }
}
