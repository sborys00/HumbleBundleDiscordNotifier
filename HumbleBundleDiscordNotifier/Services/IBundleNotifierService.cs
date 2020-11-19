namespace HumbleBundleDiscordNotifier.Services
{
    interface IBundleNotifierService
    {
        void Run();
        void Stop();
    }
}