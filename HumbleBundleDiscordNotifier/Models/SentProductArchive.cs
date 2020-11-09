using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace HumbleBundleDiscordNotifier.Models
{
    public class SentProductArchive
    {
        private readonly IConfiguration _config;
        private readonly string filePath;

        public SentProductArchive(IConfiguration config)
        {
            _config = config;

            filePath = _config.GetValue<string>("Archive_File_Path");
        }

        public List<UrlWithWebhooks> GetDeserializedUrls()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            string serializedUrls;
            using (StreamReader sr = new StreamReader(filePath))
                serializedUrls = sr.ReadToEnd();

            return JsonSerializer.Deserialize<List<UrlWithWebhooks>>(serializedUrls);
        }

        public void AddUrl(UrlWithWebhooks urlWithWebhooks)
        {
            if (IsUrlStored(urlWithWebhooks.Url))
                Log.Logger.Information("Given url is already stored in the archive.");
            else
            {
                List<UrlWithWebhooks> loadedUrls = GetDeserializedUrls();
                loadedUrls.Add(urlWithWebhooks);
                string serializedData = JsonSerializer.Serialize(loadedUrls);
                using (StreamWriter sw = new StreamWriter(filePath))
                    sw.Write(serializedData);
            }
        }

        public bool IsUrlStored(string url)
        {
            if (!File.Exists(filePath))
                return false;

            List<UrlWithWebhooks> loadedUrls = GetDeserializedUrls();

            foreach (var stored in loadedUrls)
            {
                if (stored.Url == url)
                    return true;
            }
            return false;
        }
    }
}
