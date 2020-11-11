using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace HumbleBundleDiscordNotifier.Models
{
    class BundleNotifier
    {
        private readonly IConfiguration _config;
        private readonly IWebhookSender _sender;
        private readonly IScraper _scraper;
        private static System.Timers.Timer _timer;

        public BundleNotifier(IConfiguration config, IWebhookSender sender, IScraper scraper)
        {
            _config = config;
            _sender = sender;
            _scraper = scraper;

            _timer = new System.Timers.Timer(_config.GetValue<int>("ScanningInterval"));
            _timer.AutoReset = false;
            _timer.Elapsed += ScanningLoop;
        }

        private void ScanningLoop(Object source, ElapsedEventArgs args)
        {
            _timer.Stop();
            List<Product> products = _scraper.GetListOfProducts();
            _sender.EnqueueProducts(products);
            _timer.Start();
        }
    }
}
