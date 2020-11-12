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
using System.Text.Json;
using System.Linq;

namespace HumbleBundleDiscordNotifier.Models
{
    class WebhookSender : IWebhookSender
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

        private async void SendingLoop(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            if (_productsToSend.Count > 0)
            {
                try
                {
                    Product productToSend = _productsToSend.Dequeue();
                    await SendProduct(productToSend);
                }
                catch(Exception exception)
                {
                    Log.Logger.Error(exception.Message);
                }
            }
            else
            {
                _timer.Stop();
            }
            _timer.Start();
        }

        public void EnqueueProducts(List<Product> products)
        {
            List<UrlWithWebhooks> storedProducts = _archive.GetDeserializedUrls();
            List<Webhook> webhooks = GetWebhooks();

            foreach (Product product in products)
            {
                if (_archive.IsProductDelivered(storedProducts, webhooks, product.ProductUrl) == false && IsInQueue(product) == false)
                {
                    _productsToSend.Enqueue(product);
                }
            }

            if (_productsToSend.Count > 0 && _timer.Enabled == false)
            {
                _timer.Start();
            }
        }

        private async Task SendProduct(Product product)
        {
            IConfigurationSection cfgColors = _config.GetSection("BackgroundColors");

            List<Webhook> webhooks = GetWebhooks();
            WebhookPayload payload = new WebhookPayload(product);
            Dictionary<string, int> colors = cfgColors.Get<Dictionary<string, int>>();
            string[] hashes = _archive.GetWebhookHashes(product.ProductUrl);

            foreach(Embed embed in payload.embeds)
            {
                string[] elems = product.MachineName.Split("_");
                string key = elems[elems.Length-1];
                if(colors.ContainsKey(key))
                {
                    embed.color = colors[key];
                }
            }

            List<Task> tasks = new List<Task>();

            foreach (Webhook wh in webhooks)
            {
                if(hashes.Contains(wh.Hash) == false)
                    tasks.Add(SendWebhook(wh.url, payload));
            }

            try
            {
                await Task.WhenAll(tasks);
            }
            catch(Exception e)
            {
                Log.Logger.Error(e.Message);
            }
            finally 
            { 
                UrlWithWebhooks productLabel = new UrlWithWebhooks();
                productLabel.Url = product.ProductUrl;

                for (int i = 0; i < tasks.Count; i++)
                {
                    if(tasks[i].IsCompletedSuccessfully)
                    {
                        productLabel.Webhooks.Add(webhooks[i]);
                    }
                }
                if(productLabel.Webhooks.Count > 0)
                {
                    _archive.AddUrl(productLabel);
                }
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
                            Log.Logger.Warning($"Payload could not be sent");
                            throw new Exception(responseContent);
                        }
                    }
                }
            }
        }

        private bool IsInQueue(Product product)
        {
            return _productsToSend.Any(p => p.ProductUrl == product.ProductUrl);
        }

        private List<Webhook> GetWebhooks()
        {
            IConfigurationSection cfg = _config.GetSection("Webhooks");
            string[] webhookUrls = _config.GetSection("Webhooks").Get<String[]>();
            return Webhook.GenerateWebhooks(webhookUrls);
        }
    }
}