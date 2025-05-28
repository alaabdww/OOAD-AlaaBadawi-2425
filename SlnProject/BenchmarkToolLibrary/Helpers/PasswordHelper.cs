using System;
using System.Security.Cryptography;
using System.Text;

namespace BenchmarkToolLibrary.Helpers
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Hasht het opgegeven wachtwoord met SHA256 en geeft de Base64-string (zoals in je database).
        /// </summary>
        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash); // ✅ Base64 terugzetten
            }
        }

        /// <summary>
        /// Vergelijkt een cleartext wachtwoord met een opgeslagen hash.
        /// </summary>
        public static bool VerifyPassword(string password, string hash)
        {
            string hashInput = HashPassword(password);
            return hashInput == hash;
        }
    }
}
