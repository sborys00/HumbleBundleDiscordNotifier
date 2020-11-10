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
            var json = "{" + "\"machine_name\": \"voiceswarhammerblacklibrary_bookbundle\"," +
                "\"product_url\": \"/books/voices-warhammer-2020-black-library-books\"," +
                "\"high_res_tile_image\": \"https://hb.imgix.net/7a1e2b07e8c1ec9fb30802487b748d78b6042b02.png?auto\u003dcompress,format\u0026fit\u003dcrop\u0026h\u003d600\u0026w\u003d1200\u0026s\u003dba2e892d394718913e6d4220c891ee87\"," +
                "\"tile_name\": \"Humble Audiobook Bundle: Voices of Warhammer 2020 by Black Library\"," +
                "\"type\": \"bundle\"," +
                "\"start_date|datetime\": \"2020-10-21T18:00:00\"," +
                "\"end_date|datetime\": \"2020-11-11T19:00:00\"" +
                "}";
            Product product = JsonSerializer.Deserialize<Product>(json);

            string expectedMachineName = "voiceswarhammerblacklibrary_bookbundle";
            string expectedProductUrl = "/books/voices-warhammer-2020-black-library-books";
            string expectedProductName = "Humble Audiobook Bundle: Voices of Warhammer 2020 by Black Library";
            string expectedProductThumbnailUrl = "https://hb.imgix.net/7a1e2b07e8c1ec9fb30802487b748d78b6042b02.png?auto\u003dcompress,format\u0026fit\u003dcrop\u0026h\u003d600\u0026w\u003d1200\u0026s\u003dba2e892d394718913e6d4220c891ee87";
            string expectedType = "bundle";
            string expectedStartDate = "2020-10-21T18:00:00";
            string expectedEndDate = "2020-11-11T19:00:00";

            Assert.Equal(expectedMachineName, product.MachineName);
            Assert.Equal(expectedProductUrl, product.ProductUrl);
            Assert.Equal(expectedProductName, product.ProductName);
            Assert.Equal(expectedProductThumbnailUrl, product.ProductThumbnailUrl);
            Assert.Equal(expectedType, product.Type);
            Assert.Equal(expectedStartDate, product.StartDate);
            Assert.Equal(expectedEndDate, product.EndDate);
        }
    }
}
