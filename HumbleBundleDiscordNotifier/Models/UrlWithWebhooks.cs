using System;
using System.Collections.Generic;
using System.Text;

namespace HumbleBundleDiscordNotifier.Models
{
    class UrlWithWebhooks
    {
        public readonly string Url;
        public readonly List<Webhook> Webhooks;

        public UrlWithWebhooks(string url, List<Webhook> webhooks)
        {
            Url = url;
            Webhooks = new List<Webhook>(webhooks);
        }
    }
}
