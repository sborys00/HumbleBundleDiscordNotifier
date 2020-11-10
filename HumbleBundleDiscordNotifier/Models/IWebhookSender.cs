using System.Collections.Generic;

namespace HumbleBundleDiscordNotifier.Models
{
    interface IWebhookSender
    {
        void EnqueueProducts(List<Product> products);
    }
}