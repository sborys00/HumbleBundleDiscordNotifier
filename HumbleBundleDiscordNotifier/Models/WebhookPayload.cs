using System;
using System.Collections.Generic;
using System.Text.Json;

namespace HumbleBundleDiscordNotifier.Models
{
    public class WebhookPayload
    {
        public string content { get; set; }
        public List<Embed> embeds { get; set; }

        public WebhookPayload() { }
        public WebhookPayload(Product product)
        {
            Embed embed = new Embed();
            embed.title = product.ProductName;
            embed.description = product.ProductDescription;
            embed.url = "https://www.humblebundle.com" + product.ProductUrl;

            embed.author = new Author("HumbleBundle.com");
            embed.image = new Picture(product.ProductThumbnailUrl);
            embed.footer = new Footer(DateTime.Parse(product.StartDate).ToString());

            embeds = new List<Embed>();
            embeds.Add(embed);
        }

        public WebhookPayload(Product product, string customMessage) : this (product)
        {
            content = customMessage;
        }

        public string SerializePayload()
        {
            return JsonSerializer.Serialize(this);
        }

    }

    public class Embed
    {
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public int color { get; set; }

        public Author author { get; set; }
        public Picture thumbnail { get; set; }
        public Picture image { get; set; }
        public Footer footer { get; set; }
        public List<Field> fields { get; set; }
    }

    public class Author
    {
        public string name { get; set; }

        public Author() { }
        public Author(string name)
        {
            this.name = name;
        }
    }

    public class Footer
    {
        public string text { get; set; }

        public Footer() { }
        public Footer(string text)
        {
            this.text = text;
        }
    }

    public class Picture
    {
        public string url { get; set; }

        public Picture() { }
        public Picture(string url)
        {
            this.url = url;
        }
    }

    public class Field
    {
        public string name { get; set; }
        public string value { get; set; }
        public bool inline { get; set; }
    }
}

