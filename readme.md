# Subtitle Downloader

This repository contains the following projects:

* OpenSubtitlesLibrary\
  This is a .Net Core client library implementation for the [v1 REST API](https://www.opensubtitles.com/docs/api/html/index.htm) provided by [OpenSubtitles.com](opensubtitles.com).

* SubtitleDownloader\
  A .Net Core console application which uses the `OpenSubtitlesLibrary` to provide a CLI for downloading subtitles from [OpenSubtitles.com](opensubtitles.com).

## 1. Open Subtitles library

### 1.1 Supported Open Subtitles v1 API features

The following functionality of the `OpenSubtitlesLibrary` is supported:

* [Authentication](https://www.opensubtitles.com/docs/api/html/index.htm#create-session-and-token)
* [Download subtitle file](https://www.opensubtitles.com/docs/api/html/index.htm#download-subtitle-file)
* [Find subtitles for a video file](https://www.opensubtitles.com/docs/api/html/index.htm#find-subtitles-for-a-video-file).
* [Get user data](https://www.opensubtitles.com/docs/api/html/index.htm#get-user-data)

### 1.2 Notes

On request I'll try to make the library available in a separate repository and make the library available as a NuGet package.

## 2. Subtitle Downloader CLI app

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

1. The movie has a hash match on [OpenSubtitles.com](opensubtitles.com)
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