using SubtitleDownloader.Configuration;
using System.Threading.Tasks;

namespace SubtitleDownloader.Services.Interfaces
{
    public interface IOpenSubtitlesService
    {
        SubtitleDownloaderSettings Settings { get; }

        Task RunAsync(string[] args);
    }
}
