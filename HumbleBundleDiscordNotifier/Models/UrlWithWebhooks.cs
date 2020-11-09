using System;
using System.Collections.Generic;
using System.Text;

namespace HumbleBundleDiscordNotifier.Models
{
    class UrlWithWebhooks
    {
        public string Url { get; set; }
        public List<Webhook> Webhooks { get; set; }

        public UrlWithWebhooks(string url, List<Webhook> webhooks)
        {
            Url = url;
            Webhooks = new List<Webhook>(webhooks);
        }
    }
}
