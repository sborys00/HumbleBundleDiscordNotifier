using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace HumbleBundleDiscordNotifier.Models
{
    [Serializable]
    public class Webhook
    {
        public string Hash { get; set; }
        public string url { get; set; }

        public Webhook() { }

        public Webhook(string url)
        {
            this.url = url;
            Hash = GetHashString(url);
        }

        static public List<Webhook> GenerateWebhooks(string[] urls)
        {
            List<Webhook> webhooks = new List<Webhook>();
            foreach(string url in urls)
            {
                webhooks.Add(new Webhook(url));
            }
            return webhooks;
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
