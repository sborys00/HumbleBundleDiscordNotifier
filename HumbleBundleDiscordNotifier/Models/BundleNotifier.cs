using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace HumbleBundleDiscordNotifier.Models
{
    class BundleNotifier : IBundleNotifier
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

        public void Run()
        {
            _timer.Start();
            ScanningLoop(null, null);
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void ScanningLoop(Object source, ElapsedEventArgs args)
        {
            _timer.Stop();

            try
            {
                Log.Logger.Information("Looking for new products");
                List<Product> products = _scraper.GetListOfProducts();
                _sender.EnqueueProducts(products.FindAll(p => p.Type == "bundle" || p.Type == "monthly"));
            }
            catch(Exception e)
            {
                Log.Logger.Error(e.Message);
            }

            _timer.Start();
        }
    }
}
