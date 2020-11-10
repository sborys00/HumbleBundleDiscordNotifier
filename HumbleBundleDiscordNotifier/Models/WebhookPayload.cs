using System;
using System.Collections.Generic;
using System.Text;

namespace HumbleBundleDiscordNotifier.Models
{
    class WebhookPayload
    {
        public string content { get; set; }
        public List<Embed> embeds { get; set; }

    }

    class Embed
    {
        public string title { get; set; }
        public string description { get; set; }
        public int color { get; set; }

        public Author author { get; set; }
        public Picture thumbnail { get; set; }
        public Picture image { get; set; }
        public Footer footer { get; set; }
    }

    class Author
    {
        public string name { get; set; }

        public Author(string name)
        {
            this.name = name;
        }
    }

    class Footer
    {
        public string text { get; set; }

        public Footer(string text)
        {
            this.text = text;
        }
    }

    class Picture
    {
        public string url { get; set; }

        public Picture(string url)
        {
            this.url = url;
        }
    }

    class Field
    {
        public string name { get; set; }
        public string value { get; set; }
        public bool inline { get; set; }
    }
}

