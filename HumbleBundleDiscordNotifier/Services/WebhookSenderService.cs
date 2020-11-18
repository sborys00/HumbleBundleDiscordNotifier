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
    class WebhookSenderService : IWebhookSenderService
    {
        private readonly Queue<Product> _productsToSend;
        private readonly IConfiguration _config;
        private readonly ISentProductArchive _archive;
        private readonly ICustomWebClient _webClient;
        private readonly Timer _timer;

        public WebhookSenderService(IConfiguration config, ISentProductArchive archive, ICustomWebClient webClient)
        {
            _productsToSend = new Queue<Product>();
            _config = config;
            _archive = archive;
            _webClient = webClient;

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
                    Log.Logger.Information(productToSend.ProductName + " sent successfully.");
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

            _timer.Interval = _config.GetValue<int>("SendingInterval");

            int newProductsCount = 0;
            foreach (Product product in products)
            {
                if (_archive.IsProductDelivered(storedProducts, webhooks, product.ProductUrl) == false && IsInQueue(product) == false)
                {
                    _productsToSend.Enqueue(product);
                    newProductsCount++;
                }
            }
            if(newProductsCount > 0)
            {
                Log.Logger.Information($"{newProductsCount} new products found and enqueued to send");
            }

            if (_productsToSend.Count > 0 && _timer.Enabled == false)
            {
                SendingLoop(null, null);
            }
        }

        private async Task SendProduct(Product product)
        {
            IConfigurationSection cfgColors = _config.GetSection("BackgroundColors");

            List<Webhook> webhooks = GetWebhooks();
            WebhookPayload payload = new WebhookPayload(product);
            Dictionary<string, int> colors = cfgColors.Get<Dictionary<string, int>>();
            List<Webhook> webhooksInArchive = _archive.GetWebhooksOfProduct(product.ProductUrl);

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
            List<Webhook> sentWebhooks = new List<Webhook>(); 

            try
            {
                foreach (Webhook wh in webhooks)
                {
                    if (webhooksInArchive.Any(w => w.Hash == wh.Hash) == false)
                    {
                        tasks.Add(SendWebhook(wh.url, payload));
                        sentWebhooks.Add(wh);
                    }
                }
                await Task.WhenAll(tasks);
            }
            catch(Exception e)
            {
                Log.Logger.Error(e.Message);
            }
            finally 
            { 
                UrlWithWebhooks productLabel = new UrlWithWebhooks();
                productLabel.Webhooks = webhooksInArchive;
                productLabel.Url = product.ProductUrl;

                for (int i = 0; i < tasks.Count; i++)
                {
                    if(tasks[i].IsCompletedSuccessfully)
                    {
                        productLabel.Webhooks.Add(sentWebhooks[i]);
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
            var httpContent = new StringContent(payload.SerializePayload(), Encoding.UTF8, "application/json");

            var httpResponse = await _webClient.PostAsync(url, httpContent);

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