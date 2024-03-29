﻿using System;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace HumbleBundleDiscordNotifier.Models
{
    public class Product
    {
        [JsonPropertyName("machine_name")]
        public string MachineName { get; set; }

        [JsonPropertyName("product_url")]
        public string ProductUrl { get; set; }

        [JsonPropertyName("tile_name")]
        public string ProductName { get; set; }

        [JsonPropertyName("detailed_marketing_blurb")]
        public string ProductDescription { get; set; }

        [JsonPropertyName("high_res_tile_image")]
        public string ProductThumbnailUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("start_date|datetime")]
        public string StartDate { get; set; }

        [JsonPropertyName("end_date|datetime")]
        public string EndDate { get; set; }

        public void ClearTagsAndEntities()
        {
            if(ProductName != null)
                ProductName = HtmlEntity.DeEntitize(Regex.Replace(ProductName, "<.*?>", String.Empty));
            if(ProductDescription != null)
                ProductDescription = HtmlEntity.DeEntitize(Regex.Replace(ProductDescription, "<.*?>", String.Empty));
        }

        public override string ToString()
        {
            return this.ProductName;
        }
    }
}
