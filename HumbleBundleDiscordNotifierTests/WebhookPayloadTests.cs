using System;
using System.Collections.Generic;
using Xunit;
using HumbleBundleDiscordNotifier.Models;
using System.IO;
using System.Text.Json;

namespace HumbleBundleDiscordNotifierTests
{
    public class WebhookPayloadTests
    {
        private readonly List<Product> _products;

        public WebhookPayloadTests()
        {
            //loads example products
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "/TestJsonFiles/ExampleProducts.json"))
            {
                string json = sr.ReadToEnd();
                _products = JsonSerializer.Deserialize<List<Product>>(json);
            }
        }

        [Fact]
        public void SerializePayload_ShouldWork()
        {
            Product product = _products[0];
            WebhookPayload payload = new WebhookPayload(product);
            string json = payload.SerializePayload();
            Assert.True(json.Length > 0);
        }
    }
}
