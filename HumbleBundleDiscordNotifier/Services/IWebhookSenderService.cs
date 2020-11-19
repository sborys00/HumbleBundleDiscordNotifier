using System.Collections.Generic;
using HumbleBundleDiscordNotifier.Models;

namespace HumbleBundleDiscordNotifier.Services
{
    public interface IWebhookSenderService
    {
        void EnqueueProducts(List<Product> products);
    }
}