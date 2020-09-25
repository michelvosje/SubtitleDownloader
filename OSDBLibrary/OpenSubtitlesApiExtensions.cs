using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OpenSubtitles.Interfaces;
using System.Net;
using System.Net.Http;

namespace OpenSubtitles
{
    public static class OpensubtitlesApiExtensions
    {
        /// <summary>
        /// Adds all the required services including <see cref="OpenSubtitlesApi"/> for
        /// the OpenSubtitlesLibrary.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddOpenSubtitlesApiServices(
            this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IOpenSubtitlesApi, OpenSubtitlesApi>();
            serviceCollection.TryAddSingleton<HttpClient>();
            serviceCollection.TryAddSingleton<WebClient>();

            return serviceCollection;
        }
    }
}
