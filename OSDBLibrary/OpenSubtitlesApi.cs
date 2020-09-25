using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenSubtitles.Enums;
using OpenSubtitles.Exceptions;
using OpenSubtitles.Interfaces;
using OpenSubtitles.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OpenSubtitles
{
    /// <summary>
    /// Implementation of <see cref="IOpenSubtitlesApi"> to consume the OpenSubtitles v1 REST API.
    /// </summary>
    public class OpenSubtitlesApi : IOpenSubtitlesApi
    {
        private static readonly string BASE_URL =
            $"{OpenSubtitlesApiConstants.SCHEMA}://{OpenSubtitlesApiConstants.HOST}/{OpenSubtitlesApiConstants.API_V1_ROUTE}";

        private readonly ILogger<OpenSubtitlesApi> logger;

        /// <summary>
        /// Holds an authentication token after a user has logged in.
        /// </summary>
        private AuthenticationResponse AuthenticationResponse { get; set; }

        /// <summary>
        /// Whether a user has logged in.
        /// </summary>
        /// <remarks>
        /// Login status is determinated by the availability of an authentication token
        /// in <see cref="AuthenticationResponse"/>.
        /// </remarks>
        public bool IsLoggedIn => AuthenticationResponse != null;

        /// <summary>
        /// <see cref="HttpClient"/> which is used to send OpenSubtitles API requests.
        /// </summary>
        private readonly HttpClient httpClient;

        public OpenSubtitlesApi(ILogger<OpenSubtitlesApi> logger, HttpClient httpClient)
        {
            this.logger = logger;
            this.httpClient = httpClient;

            httpClient.DefaultRequestHeaders.Accept.Add(
                MediaTypeWithQualityHeaderValue.Parse(OpenSubtitlesApiConstants.CONTENT_TYPE)
            );
        }

        /// <summary>
        /// Requests authentication token for a specific user.
        /// </summary>
        /// <param name="authenticationRequest">Authentication information.</param>
        /// <returns>Authentication token.</returns>
        public async Task<AuthenticationResponse> LoginAsync(
            AuthenticationRequest authenticationRequest)
        {
            Logout();

            var requestUri = FormatUri("login");
            var requestContent = new StringContent(JsonConvert.SerializeObject(authenticationRequest));

            requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(OpenSubtitlesApiConstants.CONTENT_TYPE);

            logger.LogInformation($"Sending login request for user: {authenticationRequest.Username}");

            var response = await httpClient.PostAsync(requestUri, requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                Login(JsonConvert.DeserializeObject<AuthenticationResponse>(responseContent));
            else
                throw new OpenSubtitlesAuthenticationException("Failed to authenticate with Open Subtitles API.");

            return AuthenticationResponse;
        }

        /// <summary>
        /// Requests download information of a specific subtitle.
        /// </summary>
        /// <param name="downloadRequest">Identifies the specific subtitle.</param>
        /// <returns>Information where to download the subtitle.</returns>
        public async Task<DownloadResponse> DownloadAsync(
            DownloadRequest downloadRequest)
        {
            RequireLoggedIn();

            var requestUri = FormatUri("download");
            var requestContent = new StringContent(JsonConvert.SerializeObject(downloadRequest));

            requestContent.Headers.ContentType = MediaTypeHeaderValue.Parse(OpenSubtitlesApiConstants.CONTENT_TYPE);

            logger.LogInformation($"Sending download request for FileId: {downloadRequest.FileId}");

            var response = await httpClient.PostAsync(requestUri, requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<DownloadResponse>(responseContent);
        }

        /// <summary>
        /// Requests an overview of available subtitles.
        /// </summary>
        /// <param name="findRequest">Search criteria.</param>
        /// <returns>Overview of available subtitles.</returns>
        public async Task<FindResponse> FindAsync(
            FindRequest findRequest)
        {
            RequireLoggedIn();

            var requestParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(findRequest.MovieHash))
                requestParams.Add("moviehash", findRequest.MovieHash);

            if (!string.IsNullOrEmpty(findRequest.ImdbId))
                requestParams.Add("imdb_id", findRequest.ImdbId);

            if (!string.IsNullOrEmpty(findRequest.TmdbId))
                requestParams.Add("tmdb_id", findRequest.TmdbId);

            if (findRequest.Type != TypeFilterEnum.Ignore)
                requestParams.Add("type", findRequest.Type.Format());

            if (!string.IsNullOrEmpty(findRequest.Query))
                requestParams.Add("query", findRequest.Query);

            if (!string.IsNullOrEmpty(findRequest.Languages))
                requestParams.Add("languages", findRequest.Languages);

            if (findRequest.HearingImpaired != FilterEnum.Ignore)
                requestParams.Add("hearing_impaired", findRequest.HearingImpaired.Format());

            if (findRequest.TrustedSources != FilterEnum.Ignore)
                requestParams.Add("trusted_sources", findRequest.TrustedSources.Format());

            if (findRequest.MachineTranslated != FilterEnum.Ignore)
                requestParams.Add("machine_translated", findRequest.MachineTranslated.Format());

            if (findRequest.AiTranslated != FilterEnum.Ignore)
                requestParams.Add("ai_translated", findRequest.AiTranslated.Format());

            if (!string.IsNullOrEmpty(findRequest.OrderBy))
                requestParams.Add("order_by", findRequest.OrderBy);

            if (!string.IsNullOrEmpty(findRequest.OrderDirection))
                requestParams.Add("order_direction", findRequest.OrderDirection);

            var requestUri = FormatUri("find", requestParams);

            logger.LogInformation($"Sending find request for movie hash: {findRequest.MovieHash}");

            var response = await httpClient.GetAsync(requestUri);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<FindResponse>(responseContent);
        }

        /// <summary>
        /// Requests user information.
        /// </summary>
        /// <returns>User information.</returns>
        public async Task<InfoUserResponse> GetUserAsync()
        {
            RequireLoggedIn();

            var requestUri = FormatUri("infos/user");

            logger.LogInformation("Sending user info request.");

            var response = await httpClient.GetAsync(requestUri);
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<InfoUserResponse>(responseContent);
        }

        /// <summary>
        /// Changes the status to logged in.
        /// </summary>
        /// <param name="authenticationResponse">Contains the authentication token.</param>
        private void Login(AuthenticationResponse authenticationResponse)
        {
            if (authenticationResponse != null)
            {
                AuthenticationResponse = authenticationResponse;

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    OpenSubtitlesApiConstants.TOKEN_HEADER, AuthenticationResponse.Token
                );

                logger.LogInformation($"Logged in user with id: {AuthenticationResponse.User.UserId}.");
            }
        }

        /// <summary>
        /// Reset the <see cref="AuthenticationResponse"/> which stores the authentication token.
        /// </summary>
        private void Logout()
        {
            if (IsLoggedIn)
            {
                logger.LogInformation($"Logged out user with id: {AuthenticationResponse.User.UserId}.");

                AuthenticationResponse = null;
            }
        }

        /// <summary>
        /// Throws an <see cref="OpenSubtitlesNotAuthenticatedException"/> when the user is not logged in.
        /// </summary>
        private void RequireLoggedIn()
        {
            if (!IsLoggedIn)
            {
                throw new OpenSubtitlesNotAuthenticatedException("Not logged in.");
            }
        }

        /// <summary>
        /// Formats a route for requesting the OpenSubtitles REST API.
        /// </summary>
        /// <param name="route">The route <see cref="string"/>.</param>
        /// <returns>Formatted OpenSubtitles API route.</returns>
        private Uri FormatUri(string route)
        {
            return new Uri($"{BASE_URL}/{route}");
        }

        /// <summary>
        /// Formats a route including query parameters for requesting the OpenSubtitles REST API.
        /// </summary>
        /// <param name="route">The route <see cref="string"/></param>
        /// <param name="queryParams">Contains the query parameters.</param>
        /// <returns>Formatted OpenSubtitles API route.</returns>
        private Uri FormatUri(string route, Dictionary<string, string> queryParams)
        {
            var url = $"{BASE_URL}/{route}";

            return new Uri(QueryHelpers.AddQueryString(url, queryParams));
        }
    }
}
