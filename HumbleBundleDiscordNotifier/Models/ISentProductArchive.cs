﻿using System.Collections.Generic;

namespace HumbleBundleDiscordNotifier.Models
{
    public interface ISentProductArchive
    {
        void AddUrl(UrlWithWebhooks urlWithWebhooks);
        bool IsUrlStored(List<UrlWithWebhooks> storedWebhooks, string url);
        bool IsProductDelivered(List<UrlWithWebhooks> storedUrls, List<Webhook> webhooks, string productUrl);
        public List<UrlWithWebhooks> GetDeserializedUrls();
        public string[] GetWebhookHashes(string productUrl);
    }
}