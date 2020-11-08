using System;
using System.Text.Json.Serialization;

namespace HumbleBundleDiscordNotifier.Models
{
    class Product
    {
        [JsonPropertyName("machine_name")]
        public string MachineName { get; set; }

        [JsonPropertyName("product_url")]
        public string ProductUrl { get; set; }

        [JsonPropertyName("tile_name")]
        public string ProductName { get; set; }

        [JsonPropertyName("high_res_tile_image")]
        public string ProductThumbnailUrl { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("start_date|datetime")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end_date|datetime")]
        public DateTime EndDate { get; set; }
    }
}
