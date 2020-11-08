using System.Collections.Generic;

namespace HumbleBundleDiscordNotifier.Models
{
    interface IScraper
    {
        List<Product> GetListOfProducts();
    }
}