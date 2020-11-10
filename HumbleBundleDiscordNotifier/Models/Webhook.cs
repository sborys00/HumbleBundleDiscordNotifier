using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HumbleBundleDiscordNotifier.Models
{
    [Serializable]
    public class Webhook
    {
        public readonly string Hash;
        public readonly string url;

        public Webhook(string url)
        {
            this.url = url;
            Hash = GetHashString(url);
        }

        private byte[] GetHash(string input)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
        }

        private string GetHashString(string input)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(input))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
