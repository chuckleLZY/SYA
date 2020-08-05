using System;
using System.Text;
using System.Security.Cryptography;

namespace SyaApi.Plugins
{
    public static class Sha512Hmac
    {
        private static string Key { get; set; }

        static Sha512Hmac()
        {
            Key = ConfigReader.GetConfig("Pepper");
        }

        public static string HashPassword(string password)
        {
            using var hasher = new HMACSHA512(Encoding.UTF8.GetBytes(Key));
            var hash = hasher.ComputeHash(Encoding.UTF8.GetBytes(password));

            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
