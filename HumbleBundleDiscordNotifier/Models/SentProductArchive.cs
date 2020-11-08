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

        public List<string> GetDeserializedWebhooks()
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }

            string serializedWebhooks;
            using (StreamReader sr = new StreamReader(filePath))
                serializedWebhooks = sr.ReadToEnd();

            return JsonSerializer.Deserialize<List<string>>(serializedWebhooks);
        }

        public void AddWebhook(string webhook)
        {
            if (IsWebhookStored(webhook))
            {
                // log that information
                return;
            }
            else
            {
                List<string> loadedWebhooks = GetDeserializedWebhooks();
                loadedWebhooks.Add(webhook);
                string serializedData = JsonSerializer.Serialize(loadedWebhooks);
                using (StreamWriter sw = new StreamWriter(filePath))
                    sw.Write(serializedData);
            }
        }

        public bool IsWebhookStored(string webhook)
        {
            if (!File.Exists(filePath))
                return false;

            List<string> loadedWebhooks = GetDeserializedWebhooks();

            foreach (var storedWebhook in loadedWebhooks)
            {
                if (storedWebhook == webhook)
                    return true;
            }
            return false;
        }
    }
}
