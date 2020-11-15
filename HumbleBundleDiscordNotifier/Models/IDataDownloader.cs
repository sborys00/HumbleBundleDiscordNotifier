namespace HumbleBundleDiscordNotifier.Models
{
    public interface IDataDownloader
    {
        string GetWebsite(string url);
    }
}