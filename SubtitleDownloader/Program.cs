using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenSubtitles;
using SubtitleDownloader.Configuration;
using SubtitleDownloader.Services.Interfaces;
using SubtitleDownloader.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SubtitleDownloader
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var parameters = ReadParameters(args);

            if (!parameters.PrintHelp)
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true)
                    .AddJsonFile(SubtitleDownloaderSettingsLocator.GetUserSettingsFileName(), true)
                    .Build();

                var serviceProvider = new ServiceCollection()
                    .Configure<SubtitleDownloaderSettings>(configuration)
                    .AddLogging(config => {
                        config.ClearProviders();
                        config.AddConsole();
                        config.AddConfiguration(configuration.GetSection("Logging"));
                    })
                    .AddOpenSubtitlesApiServices()
                    .AddSubtitleDownloaderServices()
                    .AddSingleton(parameters)
                    .BuildServiceProvider();

                await serviceProvider.GetService<IOpenSubtitlesService>()
                    .RunAsync(args);
            }
            else
            {
                PrintHelp();
            }
        }

        public static ProgramParameters ReadParameters(string[] args)
        {
            var parameters = new ProgramParameters();

            try
            {
                if (args.Length > 0)
                {
                    if (args.Length > 1)
                    {
                        if (args[0] == "config")
                        {
                            if (args.Length == 2 && args[1] == "clear")
                                parameters.ClearUserConfiguration = true;
                            else if (args.Length == 2 && args[1] == "user")
                                parameters.ConfigureUser = true;
                            else if (args.Length == 2 && args[1] == "language")
                                parameters.ConfigureLanguageFilter = true;
                            else
                                parameters.PrintHelp = true;
                        }
                        else if (args[1] == "download")
                        {
                            parameters.DownloadSpecificSubtitles = true;
                            parameters.MovieFileName = args[0];
                            parameters.SubtitlesId = args[2];
                        }
                        else if (args[1] == "list")
                        {
                            parameters.ListSubtitles = true;
                            parameters.MovieFileName = args[0];
                        }
                        else
                            parameters.PrintHelp = true;
                    }
                    else if (args.Length == 1)
                    {
                        if (args[0] == "status")
                            parameters.ShowStatus = true;
                        else
                        {
                            parameters.DownloadSubtitles = true;
                            parameters.MovieFileName = args[0];
                        }
                    }
                    else
                        parameters.PrintHelp = true;
                }
                else
                    parameters.PrintHelp = true;
            }
            catch
            {
                parameters.PrintHelp = true;
            }

            return parameters;
        }

        private static void PrintHelp()
        {
            Console.WriteLine("SubtitleDownloader [<movie>] [<command>] [<option>]");
            Console.WriteLine();
            Console.WriteLine("Available commands:");
            Console.WriteLine();
            Console.WriteLine("  config clear\t\t\t\tClear configurations.");
            Console.WriteLine("  config user\t\t\t\tConfigure user.");
            Console.WriteLine("  config language\t\t\tConfigure language filter.");
            Console.WriteLine();
            Console.WriteLine("  <movie>\t\t\t\tAutomatically download recommended subtitles file.");
            Console.WriteLine("  <movie> list\t\t\t\tList available subtitles.");
            Console.WriteLine("  <movie> download <subtitlesId>\tDownload specific subtitles by providing the subtitles ID.");
        }
    }
}
