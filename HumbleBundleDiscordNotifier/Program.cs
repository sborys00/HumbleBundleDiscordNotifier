using HtmlAgilityPack;
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
                    services.AddTransient<ISentProductArchive, SentProductArchive>()
                    .AddTransient<IScraper, Scraper>()
                    .AddTransient<ICustomWebClient, CustomWebClient>()
                    .AddSingleton<IWebhookSender, WebhookSender>()
                    .AddSingleton<IBundleNotifier, BundleNotifier>();
                })
                .UseSerilog()
                .Build();
            Log.Logger.Information("Program loaded successfully");

            IBundleNotifier bundleNotifier = host.Services.GetRequiredService<IBundleNotifier>();
            bundleNotifier.Run();

            //Keeps the program running
            Task.Run(() => Task.Delay(Timeout.Infinite)).GetAwaiter().GetResult();
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
