using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoreSIMPLECRM.Services
{
    internal class AuthService
    {
        public static byte[] GenerateSalt(int length)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var salt = new byte[length];
                rng.GetBytes(salt);
                return salt;
            }
        }

        public static string HashPassword(string password, byte[] salt)
        {
            using (var sha = new SHA256Managed())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var passwordAndSaltBytes = new byte[passwordBytes.Length + salt.Length];
                Array.Copy(passwordBytes, 0, passwordAndSaltBytes, 0, passwordBytes.Length);
                Array.Copy(salt, 0, passwordAndSaltBytes, passwordBytes.Length, salt.Length);
                var hashBytes = sha.ComputeHash(passwordAndSaltBytes);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "");
                return hashString;
            }
        }

        public static bool ValidatePassword(string password, string hash, byte[] salt)
        {
            var passwordHash = HashPassword(password, salt);
            return passwordHash == hash;
        }

    }
}
