using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace TodoAPI.Helpers
{
    public static class PasswordHelper
    {
        public static (string, byte[]) CreateSecurePassword(string password)
        {
            var salt = CreateSalt();
            var hash = CreateHash(password, salt);

            return (hash, salt);
        }

        public static bool Validate(string password, string hash, byte[] salt) => CreateHash(password, salt) == hash;

        private static string CreateHash(string password, byte[] salt)
        {
            var hashBytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            );
            var hash = Convert.ToBase64String(hashBytes);

            return hash;
        }

        private static byte[] CreateSalt()
        {
            byte[] salt = new byte[128 / 8];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            return salt;
        }
    }
}
