namespace HumbleBundleDiscordNotifier.Models
{
    interface IBundleNotifier
    {
        void Run();
        void Stop();
    }
}