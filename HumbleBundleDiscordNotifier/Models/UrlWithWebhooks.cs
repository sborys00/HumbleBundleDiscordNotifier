using System;
using System.Collections.Generic;
using System.Text;

namespace HumbleBundleDiscordNotifier.Models
{
    [Serializable]
    public class UrlWithWebhooks
    {
        public string Url { get; set; }
        public List<Webhook> Webhooks { get; set; }

        public UrlWithWebhooks()
        {
            Webhooks = new List<Webhook>();
        }

        public UrlWithWebhooks(string url, List<Webhook> webhooks)
        {
            Url = url;
            Webhooks = new List<Webhook>(webhooks);
        }
    }
}
