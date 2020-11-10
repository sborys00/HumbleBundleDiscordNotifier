using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace HumbleBundleDiscordNotifier.Models
{
    class WebhookSender
    {
        private readonly Queue<Product> _productsToSend;
        private readonly IConfiguration _config;
        private readonly ISentProductArchive _archive;
        private readonly Timer _timer;

        public WebhookSender(IConfiguration config, ISentProductArchive archive)
        {
            _productsToSend = new Queue<Product>();
            _config = config;
            _archive = archive;

            _timer = new Timer(_config.GetValue<int>("SendingInterval"));
            _timer.Elapsed += SendingLoop;
        }

        private void SendingLoop(object sender, ElapsedEventArgs e)
        {
            if(_productsToSend.Count > 0)
            {

            }
            else
            {
                _timer.Stop();
            }
        }

        public void EnqueueProducts(List<Product> products)
        {
            List<UrlWithWebhooks> storedProducts = _archive.GetDeserializedUrls();

            foreach(Product product in products)
            {
                if (_archive.IsUrlStored(storedProducts, product.ProductUrl) == false)
                {
                    _productsToSend.Enqueue(product);
                }
            }

            if(_productsToSend.Count > 0 && _timer.Enabled == false)
            {
                _timer.Start();
            }
        }

        private async Task SendWebhook(string url, WebhookPayload payload)
        {
            using (WebClient webClient = new WebClient())
            {
                var httpContent = new StringContent(payload.SerializePayload(), Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var httpResponse = await httpClient.PostAsync(url, httpContent);

                    if (httpResponse.Content != null)
                    {
                        string responseContent = await httpResponse.Content.ReadAsStringAsync();
                        if (responseContent.Length > 0)
                        {
                            Log.Logger.Warning($"Payload could not be sent: ");
                            Log.Logger.Warning(responseContent);
                        }
                    }
                }
            }
        }
    }
}