using System.Net.Http;
using System.Threading.Tasks;

namespace HumbleBundleDiscordNotifier.Models
{
    public interface ICustomWebClient
    {
        string GetWebsite(string url);
        Task<HttpResponseMessage> PostAsync(string url, StringContent payload);
    }
}