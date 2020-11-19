using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace HumbleBundleDiscordNotifier.Models
{
    public class ProductArchive : IProductArchive
    {
        private readonly IConfiguration _config;
        private readonly string filePath;

        public ProductArchive(IConfiguration config)
        {
            _config = config;

            filePath = Directory.GetCurrentDirectory() + '/' + _config.GetValue<string>("Archive_File_Path");
            Log.Logger.Information("Archive file path: " + filePath);
            //creates file 
            using(StreamWriter sw = new StreamWriter(filePath, true)){}

        }
        public void AddUrl(UrlWithWebhooks urlWithWebhooks)
        {
            List<UrlWithWebhooks> loadedUrls = GetDeserializedUrls();
            var oldRecord = loadedUrls.Find(r => r.Url == urlWithWebhooks.Url);
            if (oldRecord != null)
            {
                loadedUrls.Remove(oldRecord);
            }

            loadedUrls.Add(urlWithWebhooks);
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.WriteIndented = true;
            string serializedData = JsonSerializer.Serialize(loadedUrls, options);
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(serializedData);
            }
        }

        public List<Webhook> GetWebhooksOfProduct(string productUrl)
        {
            var product = GetDeserializedUrls().Find(p => p.Url == productUrl);
            if(product != null)
            {
                List<Webhook> webhooks = product.Webhooks;
            }
            return new List<Webhook>();
        }

        public bool IsProductDelivered(List<UrlWithWebhooks> storedUrls, List<Webhook> webhooks, string productUrl)
        {
            if(IsUrlStored(storedUrls, productUrl))
            {
                UrlWithWebhooks product = storedUrls.Find(p => p.Url == productUrl);

                if (product.Webhooks.Count != webhooks.Count)
                    return false;

                foreach(Webhook wh in webhooks)
                {
                    if(product.Webhooks.Any(w => w.Hash == wh.Hash) == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public bool IsUrlStored(List<UrlWithWebhooks> storedUrls, string url)
        {
            foreach (var stored in storedUrls)
            {
                if (stored.Url == url)
                    return true;
            }
            return false;
        }

        public bool IsSentToAllWebhooks(List<UrlWithWebhooks> storedUrls, string url)
        {
            foreach (var stored in storedUrls)
            {
                if (stored.Url == url)
                    return true;
            }
            return false;
        }

        public List<UrlWithWebhooks> GetDeserializedUrls()
        {
            string serializedUrls;
            using (StreamReader sr = new StreamReader(filePath))
                serializedUrls = sr.ReadToEnd();
            if(serializedUrls.Length == 0)
            {
                return new List<UrlWithWebhooks>();
            }

            return JsonSerializer.Deserialize<List<UrlWithWebhooks>>(serializedUrls);
        }

    }
}
