using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace HumbleBundleDiscordNotifier.Models
{
    class SentProductArchive
    {
        private readonly IConfiguration _config;
        private readonly string filePath;

        public SentProductArchive(IConfiguration config)
        {
            _config = config;

            filePath = _config.GetValue<string>("Archive_File_Path");
        }

        public List<string> GetDeserializedUrls()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            string serializedUrls;
            using (StreamReader sr = new StreamReader(filePath))
                serializedUrls = sr.ReadToEnd();

            return JsonSerializer.Deserialize<List<string>>(serializedUrls);
        }

        public void AddUrl(string url)
        {
            if (IsUrlStored(url))
            {
                // log that information
                return;
            }
            else
            {
                List<string> loadedUrls = GetDeserializedUrls();
                loadedUrls.Add(url);
                string serializedData = JsonSerializer.Serialize(loadedUrls);
                using (StreamWriter sw = new StreamWriter(filePath))
                    sw.Write(serializedData);
            }
        }

        public bool IsUrlStored(string url)
        {
            if (!File.Exists(filePath))
                return false;

            List<string> loadedUrls = GetDeserializedUrls();

            foreach (var storedUrl in loadedUrls)
            {
                if (storedUrl == url)
                    return true;
            }
            return false;
        }
    }
}
