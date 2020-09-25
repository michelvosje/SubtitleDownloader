using Microsoft.Extensions.DependencyInjection;
using SubtitleDownloader.Services;
using SubtitleDownloader.Services.Interfaces;

namespace SubtitleDownloader
{
    public static class SubtitleDownloaderExtensions
    {
        public static IServiceCollection AddSubtitleDownloaderServices(
            this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddSingleton<IOpenSubtitlesService, OpenSubtitlesService>();
        }
    }
}
