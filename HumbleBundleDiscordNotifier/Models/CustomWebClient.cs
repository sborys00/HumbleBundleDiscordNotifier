using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace HumbleBundleDiscordNotifier.Models
{
    public class CustomWebClient : ICustomWebClient
    {
        public string GetWebsite(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(url);
            return doc.Text;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, StringContent payload)
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.PostAsync(url, payload);
            }
        }
    }
}
