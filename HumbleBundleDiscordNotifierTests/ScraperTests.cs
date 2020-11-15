using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using HtmlAgilityPack;
using HumbleBundleDiscordNotifier.Models;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace HumbleBundleDiscordNotifierTests
{
    public class ScraperTests
    {
        private readonly string _productsJson;

        public ScraperTests()
        {
            using (StreamReader sr = new StreamReader(Directory.GetCurrentDirectory() + "/TestJsonFiles/ExampleProducts.json"))
            {
                _productsJson = sr.ReadToEnd();
            }
        }

        [Fact]
        public void GetListOfProducts_ShouldWork()
        {
            string json = "{\"mosaic\": [{\"products\": " + _productsJson + "}, {\"products\": " + _productsJson + "}] }";
            string html = $"<script id=\"webpack-json-data\">" + json + "</script>";

            var mockCfgSection = new Mock<IConfigurationSection>();
            mockCfgSection.SetupGet(s => s.Value)
                .Returns("randomUrl");

            var mockCfg = new Mock<IConfiguration>();
            mockCfg.Setup(x => x.GetSection("WebsiteUrl")).Returns(mockCfgSection.Object);

            var mockWeb = new Mock<IDataDownloader>();
            mockWeb.Setup(x => x.GetWebsite("randomUrl"))
                .Returns(html);

            var mockScraper = new Mock<Scraper>(mockCfg.Object, mockWeb.Object);

            var listOfProducts = mockScraper.Object.GetListOfProducts();

            int expected = 8;
            int actual = listOfProducts.Count;

            Assert.Equal(expected, actual);
        }
    }
}
