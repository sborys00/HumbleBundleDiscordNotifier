using System.Collections.Generic;

namespace HumbleBundleDiscordNotifier.Models
{
    public interface ISentProductArchive
    {
        void AddUrl(UrlWithWebhooks urlWithWebhooks);
        bool IsUrlStored(List<UrlWithWebhooks> storedWebhooks, string url);
        public List<UrlWithWebhooks> GetDeserializedUrls();
    }
}