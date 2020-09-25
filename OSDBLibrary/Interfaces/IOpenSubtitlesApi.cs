using OpenSubtitles.Models;
using System.Threading.Tasks;

namespace OpenSubtitles.Interfaces
{
    /// <summary>
    /// A <see cref="IOpenSubtitlesApi"> can be used to consume the OpenSubtitles v1 REST API.
    /// </summary>
    public interface IOpenSubtitlesApi
    {
        /// <summary>
        /// Whether a user has logged in.
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// Requests authentication token for a specific user.
        /// </summary>
        /// <param name="downloadRequest">Authentication information.</param>
        /// <returns>Authentication token.</returns>
        Task<AuthenticationResponse> LoginAsync(AuthenticationRequest downloadRequest);

        /// <summary>
        /// Requests download information of a specific subtitle.
        /// </summary>
        /// <param name="downloadRequest">Identifies the specific subtitle.</param>
        /// <returns>Information where to download the subtitle.</returns>
        Task<DownloadResponse> DownloadAsync(DownloadRequest downloadRequest);

        /// <summary>
        /// Requests an overview of available subtitles.
        /// </summary>
        /// <param name="findRequest">Search criteria.</param>
        /// <returns>Overview of available subtitles.</returns>
        Task<FindResponse> FindAsync(FindRequest findRequest);

        /// <summary>
        /// Requests user information.
        /// </summary>
        /// <returns>User information.</returns>
        Task<InfoUserResponse> GetUserAsync();
    }
}
