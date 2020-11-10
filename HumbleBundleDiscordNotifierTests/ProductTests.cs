using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.IO;
using HumbleBundleDiscordNotifier.Models;
using System.Text.Json;

namespace HumbleBundleDiscordNotifierTests
{
    public class ProductTests
    {
        private readonly string _productsJson;

        public ProductTests()
        {
            //loads example products
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "/TestJsonFiles/ExampleProducts.json"))
            {
                _productsJson = sr.ReadToEnd();                
            }
        }

        [Fact]
        public void CanBeDeserialized()
        {
            List<Product> products = JsonSerializer.Deserialize<List<Product>>(_productsJson);
        }
    }
}
