using HtmlAgilityPack;

namespace HumbleBundleDiscordNotifier.Models
{
    public class DataDownloader : IDataDownloader
    {
        public string GetWebsite(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc.Text;
        }
    }
}
