using HumbleBundleDiscordNotifier.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace HumbleBundleDiscordNotifier
{
    class Program
    {
        private static System.Timers.Timer _timer;
        private static IConfiguration _config;

        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            BuildConfig(builder);
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            var host = Host.CreateDefaultBuilder().ConfigureAppConfiguration((hostingContext, config) =>
                {
                    BuildConfig(config);
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<ISentProductArchive, SentProductArchive>();
                })
                .UseSerilog()
                .Build();
            _config = host.Services.GetRequiredService<IConfiguration>();

            _timer = new System.Timers.Timer(_config.GetValue<int>("ScanningInterval"));
            _timer.AutoReset = false;
            _timer.Elapsed += MainReccuring;
            MainReccuring(null, null);

            ISentProductArchive archive = host.Services.GetRequiredService<ISentProductArchive>();
            List<Webhook> wh = new List<Webhook>();
            wh.Add(new Webhook("testwh"));
            UrlWithWebhooks url = new UrlWithWebhooks("test", wh);
            archive.AddUrl(url);

            //Keeps the program running
            Task.Run(() => Task.Delay(Timeout.Infinite)).GetAwaiter().GetResult();
        }

        static void MainReccuring(Object source, ElapsedEventArgs args)
        {

            _timer.Start();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }
}
