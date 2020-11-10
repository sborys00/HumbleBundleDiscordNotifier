namespace HumbleBundleDiscordNotifier.Models
{
    public interface ISentProductArchive
    {
        void AddUrl(UrlWithWebhooks urlWithWebhooks);
        bool IsUrlStored(string url);
    }
}