using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BenchmarkToolLibrary.Helpers
{
    /// <summary>
    /// Bevat hulpmethoden voor het veilig hashen en vergelijken van wachtwoorden.
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Hasht het opgegeven wachtwoord met SHA256.
        /// </summary>
        /// <param name="password">Het cleartext wachtwoord</param>
        /// <returns>Base64-string van de hash</returns>
        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }


        /// <summary>
        /// Vergelijkt een cleartext wachtwoord met een opgeslagen hash.
        /// </summary>
        /// <param name="password">Het cleartext wachtwoord</param>
        /// <param name="hash">De opgeslagen hash (Base64-string)</param>
        /// <returns>True als het wachtwoord overeenkomt met de hash, anders false</returns>
        public static bool VerifyPassword(string password, string hash)
        {
            string hashInput = HashPassword(password);
            return hashInput == hash;
        }
    }
}
