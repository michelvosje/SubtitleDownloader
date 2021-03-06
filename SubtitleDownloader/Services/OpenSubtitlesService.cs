﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenSubtitles.Enums;
using OpenSubtitles.Exceptions;
using OpenSubtitles.Interfaces;
using OpenSubtitles.Models;
using OpenSubtitles.Utility;
using SubtitleDownloader.Configuration;
using SubtitleDownloader.Extensions;
using SubtitleDownloader.Services.Interfaces;
using SubtitleDownloader.Utility;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SubtitleDownloader.Services
{
    public class OpenSubtitlesService : IOpenSubtitlesService
    {
        private readonly ILogger<OpenSubtitlesService> logger;
        private readonly ProgramParameters programParameters;
        private readonly IOpenSubtitlesApi openSubtitlesApi;
        private readonly WebClient webClient;

        public SubtitleDownloaderSettings Settings { get; private set; }

        public OpenSubtitlesService(
            ILogger<OpenSubtitlesService> logger,
            ProgramParameters programParameters,
            IOptions<SubtitleDownloaderSettings> options,
            IOpenSubtitlesApi openSubtitlesApi,
            WebClient webClient)
        {
            this.logger = logger;
            this.programParameters = programParameters;
            this.openSubtitlesApi = openSubtitlesApi;
            this.webClient = webClient;

            Settings = options.Value;
        }

        public async Task RunAsync(string[] args)
        {
            if (programParameters.ClearUserConfiguration)
                await DeleteUserSettingsAsync();
            if (programParameters.ConfigureUser)
                await ConfigureUserAsync();
            else if (programParameters.ConfigureLanguageFilter)
                await ConfigureLanguageFilterAsync();
            else if (programParameters.DownloadSpecificSubtitles)
                await DownloadMovieSubtitlesAsync(programParameters.MovieFileName, programParameters.SubtitlesId);
            else if (programParameters.DownloadSubtitles)
                await DownloadMovieSubtitlesAsync(programParameters.MovieFileName);
            else if (programParameters.ListSubtitles)
                await ListMovieSubtitlesAsync(programParameters.MovieFileName);
            else if (programParameters.ShowStatus)
                await ShowUserStatusAsync();
        }

        private async Task ConfigureUserAsync()
        {
            try
            {
                Settings.OpenSubtitles = new SubtitleDownloaderSettings.OpenSubtitlesSettings();

                Console.Write("Configure Open Subtitles username: ");
                Settings.OpenSubtitles.Username = Console.ReadLine();

                if (string.IsNullOrEmpty(Settings.OpenSubtitles.Username))
                {
                    Console.WriteLine("Invalid username.");
                    return;
                }

                Console.WriteLine("Configure Open Subtitles password:");
                Settings.OpenSubtitles.Password = GetUserPassword().ToString();

                if (string.IsNullOrEmpty(Settings.OpenSubtitles.Password))
                {
                    Console.WriteLine("Invalid password.");
                    return;
                }

                await SaveUserSettingsAsync(Settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to configure user credentials.");
                logger.LogError(ex, "Failed to configure user credentials.");
            }
        }

        private async Task ConfigureLanguageFilterAsync()
        {
            try
            {
                Console.Write("Configure Open Subtitles language filter (comma separated): ");
                Settings.OpenSubtitles.LanguageFilter = Console.ReadLine();

                await SaveUserSettingsAsync(Settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to configure language filter.");
                logger.LogError(ex, "Failed to configure language filter.");
                throw;
            }
        }

        private async Task DownloadMovieSubtitlesAsync(string movieFileName)
        {
            try
            {
                var movieName = Path.GetFileNameWithoutExtension(movieFileName);

                var subtitles = await FindOpenSubtitlesByMovieAsync(movieFileName);
                var subtitlesFiltered = subtitles.Data
                    .FilterSubtitles()
                    .OrderByPrioritization(movieName)
                    .FirstOrDefault();

                if (subtitlesFiltered != null)
                {
                    await DownloadSubtitlesFromOpenSubtitlesAsync(subtitlesFiltered, movieName);
                }
                else
                {
                    logger.LogInformation("No subtitles available to download.");
                }
            }
            catch (OpenSubtitlesException ex)
            {
                Console.WriteLine("Failed to download subtitles.");
                logger.LogError(ex, $"Failed to download subtitles for movie: {movieFileName}");
                throw;
            }
        }

        private async Task DownloadMovieSubtitlesAsync(string movieFileName, string subltitlesId)
        {
            try
            {
                var movieName = Path.GetFileNameWithoutExtension(movieFileName);

                var subtitles = await FindOpenSubtitlesByMovieAsync(movieFileName);
                var subtitlesFiltered = subtitles.Data
                    .FilterSubtitles()
                    .FirstOrDefault(x => x.Id == subltitlesId);

                if (subtitlesFiltered != null)
                {
                    await DownloadSubtitlesFromOpenSubtitlesAsync(subtitlesFiltered, movieName);
                }
                else
                {
                    logger.LogInformation("No subtitles available to download.");
                }
            }
            catch (OpenSubtitlesException ex)
            {
                Console.WriteLine($"Failed to download subtitles with Id:{subltitlesId}.");
                logger.LogError(ex, $"Failed to download subtitles for movie:{movieFileName} with Id:{subltitlesId}");
                throw;
            }
        }

        /// <summary>
        /// Downloads specific subtitles.
        /// </summary>
        /// <param name="subtitles"></param>
        /// <returns></returns>
        private async Task DownloadSubtitlesFromOpenSubtitlesAsync(FindResponse.DataModel subtitles, string movieName)
        {
            var subtitleId = subtitles.Id;
            var subtitleFileId = subtitles.Attributes.Files.First().Id;
            var subtitleTitle = subtitles.Attributes.Release;

            var downloadResponse = await openSubtitlesApi.DownloadAsync(new DownloadRequest()
            {
                FileId = subtitleFileId,
                FileName = subtitleTitle
            });

            var movieFileName = $"{movieName}.{Path.GetExtension(downloadResponse.FileName)}";

            await DownloadFileAsync(downloadResponse.Link, movieFileName);
            Console.WriteLine(
                $"Downloaded subtitle with Id:{subtitleId}, Title:{subtitleTitle} to:{movieFileName}"
            );
        }

        /// <summary>
        /// List the available Open Subtitles subtitles for a specific movie.
        /// </summary>
        /// <param name="movieFileName">The name of the movie file.</param>
        /// <returns>Task which lists the available subtitles.</returns>
        private async Task ListMovieSubtitlesAsync(string movieFileName)
        {
            try
            {
                var movieName = Path.GetFileNameWithoutExtension(movieFileName);

                var subtitles = await FindOpenSubtitlesByMovieAsync(movieFileName);
                var subtitlesFiltered = subtitles.Data
                    .FilterSubtitles()
                    .OrderByPrioritization(movieName)
                    .ToList();

                if (subtitlesFiltered != null && subtitlesFiltered.Any())
                {
                    Console.Write("Available subtitles:");

                    foreach (var data in subtitlesFiltered)
                    {
                        Console.WriteLine();

                        Console.WriteLine(
                            $" - Id:{data.Id}, Title:{data.Attributes.Release}, Language:{data.Attributes.Language}, Trusted:{data.Attributes.FromTrusted}"
                        );
                        Console.WriteLine($"   Ratings:{data.Attributes.Ratings}");
                        Console.WriteLine($"   Downloaded:{data.Attributes.DownloadCount}");
                        Console.WriteLine($"   Hearing impaired:{data.Attributes.HearingImpaired}");
                        Console.WriteLine($"   AI translated:{data.Attributes.AiTranslated}");
                        Console.WriteLine($"   Machine translated:{data.Attributes.MachineTranslated.GetValueOrDefault()}");
                    }
                }
                else
                {
                    logger.LogInformation("No subtitles available.");
                }
            }
            catch (OpenSubtitlesException ex)
            {
                Console.WriteLine("Failed to list available subtitles.");
                logger.LogError(ex, $"Failed to list available subtitles for movie: {movieFileName}");
                throw;
            }
        }

        /// <summary>
        /// Prints the user status.
        /// </summary>
        /// <returns>Task which print the user status.</returns>
        private async Task ShowUserStatusAsync()
        {
            try
            {
                var userInfo = await GetOpenSubtitlesUserInfo();

                Console.WriteLine("Open Subtitles user status:");
                Console.WriteLine($" - Remaining downloads today: {userInfo.Data.RemainingDownloads}/{userInfo.Data.AllowedDownloads}");
            }
            catch (OpenSubtitlesException ex)
            {
                logger.LogError(ex, $"Failed to show Open Subtitles user status");
                throw;
            }
        }

        /// <summary>
        /// Retrieves the Open Subtitles user info.
        /// </summary>
        /// <returns>Open Subtitles user info.</returns>
        private async Task<InfoUserResponse> GetOpenSubtitlesUserInfo()
        {
            await LoginOpenSubtitles();

            return await openSubtitlesApi.GetUserAsync();
        }

        /// <summary>
        /// Find the available Open Subtitles subtitles for a specific movie.
        /// </summary>
        /// <param name="movieFileName">The name of the movie file.</param>
        /// <returns>Task which returns a <see cref="FindResponse"/> containing the available subtitles.</returns>
        private async Task<FindResponse> FindOpenSubtitlesByMovieAsync(string movieFileName)
        {
            await LoginOpenSubtitles();

            var findRequest = new FindRequest()
            {
                MovieHash = MovieHasher.ComputeMovieHash(movieFileName),
                Query = Path.GetFileNameWithoutExtension(movieFileName),
                Type = TypeFilterEnum.All,
                Languages = Settings.OpenSubtitles.LanguageFilter,
                HearingImpaired = FilterEnum.Exclude,
                TrustedSources = FilterEnum.Only,
                MachineTranslated = FilterEnum.Exclude,
                AiTranslated = FilterEnum.Exclude,
            };

            return await openSubtitlesApi.FindAsync(findRequest);
        }

        private async Task<AuthenticationResponse> LoginOpenSubtitles()
        {
            return await openSubtitlesApi.LoginAsync(new AuthenticationRequest()
            {
                Username = Settings.OpenSubtitles.Username,
                Password = Settings.OpenSubtitles.Password
            });
        }

        /// <summary>
        /// Downloads the file from <paramref name="address"/> and saves it to <paramref name="fileName"/>.
        /// </summary>
        /// <param name="address">The file to download.</param>
        /// <param name="fileName">The name of the file save the downloaded file to.</param>
        /// <returns>Task which downloads and saves the file.</returns>
        private async Task DownloadFileAsync(Uri address, string fileName)
        {
            await webClient.DownloadFileTaskAsync(
                address,
                fileName
            );
        }

        /// <summary>
        /// Lets the user enter password in console window.
        /// </summary>
        /// <returns><User password./returns>
        public string GetUserPassword()
        {
            var passwordBuilder = new StringBuilder();

            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);

                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (passwordBuilder.Length > 0)
                        passwordBuilder.Remove(passwordBuilder.Length - 1, 1);
                }
                else if (i.KeyChar != '\u0000')
                {
                    // KeyChar == '\u0000' if the key pressed does not correspond to a printable character, e.g. F1, Pause-Break, etc

                    passwordBuilder.Append(i.KeyChar);
                }
            }

            return passwordBuilder.ToString();
        }

        /// <summary>
        /// Delete the user settings.
        /// </summary>
        /// <returns>Task which deletes the user settings.</returns>
        private async Task DeleteUserSettingsAsync()
        {
            try
            {
                var settingsDirectory = SubtitleDownloaderSettingsLocator.GetUserSettingsDirectory();

                if (Directory.Exists(settingsDirectory))
                    Directory.Delete(settingsDirectory, true);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to delete user settings.");
                Console.WriteLine("Failed to delete user settings.");
                throw;
            }
        }

        /// <summary>
        /// Save user settings.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns>Task which saves the user settings.</returns>
        private async Task SaveUserSettingsAsync(SubtitleDownloaderSettings settings)
        {
            var settingsJson = JsonConvert.SerializeObject(settings, Formatting.Indented);
            var settingsDirectory = SubtitleDownloaderSettingsLocator.GetUserSettingsDirectory();
            var settingsFileName = SubtitleDownloaderSettingsLocator.GetUserSettingsFileName();

            if (!Directory.Exists(settingsDirectory))
                Directory.CreateDirectory(settingsDirectory);

            await File.WriteAllTextAsync(
                settingsFileName,
                settingsJson
            );
        }
    }
}
