namespace HumbleBundleDiscordNotifier.Models
{
    interface IBundleNotifierService
    {
        void Run();
        void Stop();
    }
}