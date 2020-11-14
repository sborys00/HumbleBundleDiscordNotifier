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

        [Fact]
        public void CorrectlyDeserializeProperties()
        {
            string expectedMachineName = "voiceswarhammerblacklibrary_bookbundle";
            string expectedProductUrl = "/books/voices-warhammer-2020-black-library-books";
            string expectedProductName = "Humble Audiobook Bundle: Voices of Warhammer 2020 by Black Library";
            string expectedProductThumbnailUrl = "https://hb.imgix.net/7a1e2b07e8c1ec9fb30802487b748d78b6042b02.png?auto\u003dcompress,format\u0026fit\u003dcrop\u0026h\u003d600\u0026w\u003d1200\u0026s\u003dba2e892d394718913e6d4220c891ee87";
            string expectedType = "bundle";
            string expectedStartDate = "2020-10-21T18:00:00";
            string expectedEndDate = "2020-11-11T19:00:00";

            var json = "{" + $"\"machine_name\": \"{expectedMachineName}\"," +
                $"\"product_url\": \"{expectedProductUrl}\"," +
                $"\"high_res_tile_image\": \"{expectedProductThumbnailUrl}\"," +
                $"\"tile_name\": \"{expectedProductName}\"," +
                $"\"type\": \"{expectedType}\"," +
                $"\"start_date|datetime\": \"{expectedStartDate}\"," +
                $"\"end_date|datetime\": \"{expectedEndDate}\"" +
                "}";

            Product product = JsonSerializer.Deserialize<Product>(json);

            Assert.Equal(expectedMachineName, product.MachineName);
            Assert.Equal(expectedProductUrl, product.ProductUrl);
            Assert.Equal(expectedProductName, product.ProductName);
            Assert.Equal(expectedProductThumbnailUrl, product.ProductThumbnailUrl);
            Assert.Equal(expectedType, product.Type);
            Assert.Equal(expectedStartDate, product.StartDate);
            Assert.Equal(expectedEndDate, product.EndDate);
        }

        [Fact]
        public void ClearTagsAndEntities_ShouldWork()
        {
            Product product = new Product{
                ProductName = "<b>Square Enix Action \u0026 Adventure Sale</b>",
                ProductDescription = "We\u0027ve teamed up with Black Library for our newest bundle! Get audiobooks like \u003cem\u003eFirst and Only\u003c/ em\u003e, \u003cem\u003eRealmslayer\u003c/ em\u003e, \u003cem\u003eSoul Wars\u003c/ em\u003e, \u003cem\u003eFor the Emperor\u003c/ em\u003e, \u003cem\u003eThe House of Night and Chain\u003c/ em\u003e, and \u003cem\u003eValdor: Birth of the Imperium\u003c/ em\u003e. Plus, your purchase will support the EveryLibrary Institute!"
            };
            product.ClearTagsAndEntities();

            string nameExpected = "Square Enix Action & Adventure Sale";
            string descriptionExpected = "We've teamed up with Black Library for our newest bundle! Get audiobooks like First and Only, Realmslayer, Soul Wars, For the Emperor, The House of Night and Chain, and Valdor: Birth of the Imperium. Plus, your purchase will support the EveryLibrary Institute!";

            Assert.Equal(nameExpected, product.ProductName);
            Assert.Equal(descriptionExpected, product.ProductDescription);

        }
    }
}
