using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using HumbleBundleDiscordNotifier.Models;
using Xunit;
using Xunit.Abstractions;

namespace HumbleBundleDiscordNotifierTests
{
    public class UrlWithWebhooksTests
    {
        private readonly ITestOutputHelper output;

        public UrlWithWebhooksTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void CanBeSerialized()
        {
            Webhook webhook = new Webhook("www.google.com");
            UrlWithWebhooks urlWithWebhooks = new UrlWithWebhooks("www.microsoft.com", new List<Webhook>() { webhook });

            JsonSerializer.Serialize(urlWithWebhooks);
        }

        [Fact]
        public void CanBeDeserialized()
        {
            string expected = "www.microsoft.com";

            string jsonString = "[" +
                "{" +
                "\"Url\": \"" + expected + "\"," +
                "\"WebHooks\": " +
                "[" +
                "{" +
                "\"Hash\": \"xyz\"," +
                "\"url\": \"www.google.com\"" +
                "}" +
                "]" +
                "}" +
                "]";

            List<UrlWithWebhooks> deserializedData = JsonSerializer.Deserialize<List<UrlWithWebhooks>>(jsonString);
            Assert.True(expected == deserializedData[0].Url);
        }
    }
}
