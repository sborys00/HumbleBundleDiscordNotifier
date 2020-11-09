using System;
using System.Collections.Generic;
using System.Text;

namespace HumbleBundleDiscordNotifier.Models
{
    class Webhook
    {
        public string Hash { get; set; }
        private string url;

        public Webhook(string url)
        {
            this.url = url;
            //Hash = hash the url
        }
    }
}
