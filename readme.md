# Subtitle Downloader

## 2. Subtitle Downloader CLI app

.Net Core console application which uses [.Net Standard Open Subtitles client library](https://github.com/michelvosje/DotNet-Standard-OpenSubtitles) to provide a CLI for downloading subtitles from [OpenSubtitles.com](https://opensubtitles.com).

### 2.1 CLI commands

```cli
SubtitleDownloader [<movie>] [<command>] [<option>]

Available commands:

  config clear                          Clear configurations.
  config user                           Configure user.
  config language                       Configure language filter.

  <movie>                               Automatically download recommended subtitles file.
  <movie> list                          List available subtitles.
  <movie> download <subtitlesId>        Download specific subtitles by providing the subtitles ID.
```

To configure user credentials:

```cli
config user
```

To configure user language filter (comma separated):

```cli
config language
```

To remove the user directory containing the user configurations:

```cli
config clear
```

To automatically download the recommended subtitles file for a movie file:

```cli
<movieFilePath>
```

To list the available subtitles for a movie file in recommended order:

```cli
<movieFilePath> list
```

To download a specific subtitles file for a movie file:

```cli
<movieFilePath> download <subtitlesId>
```

### 2.2 Where are configurations stored

On Windows 10 the user configurations are stored in:

```txt
%UserProfile%\AppData\Local\SubtitleDownloader\settings.json
```

Note that the password will be stored in plain text.

### 2.3 Which prioritization is used to automatically select subtitles

It uses the following order for prioritization:

1. The movie has a hash match on [OpenSubtitles.com](https://opensubtitles.com)
2. The name of the movie has a close match to the name of the subtitles file.
3. The subtitles are from a trusted source.
4. The subtitles are not from hearing impaired. (Sorry! Perhaps this rule should become optional.)
5. Try to prevent auto generated subtitles.

Look in the following file for more information:

```txt
.\SubtitleDownloader\Extensions\FindResponseExtensions.cs
```

In the future it might be interesting to also match on the following properties:

* Name of the movie related to the subtitles.
* Season and episode retrieved from the movie meta information.