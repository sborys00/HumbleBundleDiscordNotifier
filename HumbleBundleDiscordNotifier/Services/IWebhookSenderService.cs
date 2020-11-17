using System.Collections.Generic;

namespace HumbleBundleDiscordNotifier.Models
{
    interface IWebhookSenderService
    {
        void EnqueueProducts(List<Product> products);
    }
}