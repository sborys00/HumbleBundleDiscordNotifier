using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using HumbleBundleDiscordNotifier.Models;

namespace HumbleBundleDiscordNotifierTests
{
    public class WebhookTests
    {
        [Fact]
        public void GenerateWebhooks_ShouldWork()
        {
            string[] urls = {
                "https://www.domain.com/test1",
                "https://www.domain.com/test2",
                "https://www.domain.com/test3",
                "https://www.domain.com/test4",
                "https://www.domain.com/test5",
                "https://www.domain.com/test6",
            };
            List<Webhook> webhooks = Webhook.GenerateWebhooks(urls);
            for (int i = 0; i < urls.Length; i++)
            {
                Assert.Equal(urls[i], webhooks[i].url);
            }
        }
    }
}
