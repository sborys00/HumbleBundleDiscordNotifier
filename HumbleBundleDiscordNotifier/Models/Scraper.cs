﻿using HtmlAgilityPack;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace HumbleBundleDiscordNotifier.Models
{
    public class Scraper : IScraper
    {
        private readonly IConfiguration _config;
        private readonly string _url;
        private readonly ICustomWebClient _web;

        public Scraper(IConfiguration config, ICustomWebClient web)
        {
            _config = config;
            _web = web;
            _url = config.GetValue<string>("WebsiteUrl");
        }

        public List<Product> GetListOfProducts()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(_web.GetWebsite(_url));

            string data = ExtractDataFromHtmlDocument(doc, "webpack-json-data");

            JsonDocument json = JsonDocument.Parse(data);

            JsonElement productsJson = json.RootElement.GetProperty("mosaic")[1].GetProperty("products");
            JsonElement productsJson2 = json.RootElement.GetProperty("mosaic")[0].GetProperty("products");

            List<Product> products = JsonSerializer.Deserialize<List<Product>>(productsJson.GetRawText());
            List<Product> products2 = JsonSerializer.Deserialize<List<Product>>(productsJson2.GetRawText());

            products.AddRange(products2);

            foreach (Product product in products)
            {
                product.ClearTagsAndEntities();
            }

            return products;
        }

        private string ExtractDataFromHtmlDocument(HtmlDocument doc, string id)
        {
            string data = doc.GetElementbyId(id).InnerText;
            return data;
        }
    }
}
