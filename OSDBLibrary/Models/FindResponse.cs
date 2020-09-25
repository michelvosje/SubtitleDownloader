using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OpenSubtitles.Models
{
    [JsonObject]
    public class FindResponse
    {
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }


        [JsonProperty("total_count")]
        public int TotalCount { get; set; }


        [JsonProperty("page")]
        public int Page { get; set; }


        [JsonProperty("data")]
        public List<DataModel> Data { get; set; }

        [JsonObject]
        public class DataModel
        {
            [JsonProperty("id")]
            public string Id { get; set; }
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("attributes")]
            public AttributeModel Attributes { get; set; }
        }

        [JsonObject]
        public class AttributeModel
        {
            [JsonProperty("language")]
            public string Language { get; set; }

            [JsonProperty("download_count")]
            public int DownloadCount { get; set; }

            [JsonProperty("new_download_count")]
            public int NewDownloadCount { get; set; }

            [JsonProperty("hearing_impaired")]
            public bool HearingImpaired { get; set; }

            [JsonProperty("hd")]
            public bool HD { get; set; }

            [JsonProperty("format")]
            public string Format { get; set; }

            [JsonProperty("fps")]
            public decimal FPS { get; set; }

            [JsonProperty("votes")]
            public int Votes { get; set; }

            [JsonProperty("points")]
            public int Points { get; set; }

            [JsonProperty("ratings")]
            public decimal Ratings { get; set; }

            [JsonProperty("from_trusted")]
            public bool FromTrusted { get; set; }

            [JsonProperty("auto_translation")]
            public bool AutoTranslation { get; set; }

            [JsonProperty("ai_translated")]
            public bool AiTranslated { get; set; }

            [JsonProperty("machine_translated")]
            public Nullable<bool> MachineTranslated { get; set; }

            [JsonProperty("upload_date")]
            public DateTime UploadDate { get; set; }

            [JsonProperty("release")]
            public string Release { get; set; }

            [JsonProperty("comments")]
            public string Comments { get; set; }

            [JsonProperty("legacy_subtitle_id")]
            public string LegacySubtitleId { get; set; }

            [JsonProperty("uploader")]
            public UploaderModel Uploader { get; set; }

            [JsonProperty("feature_details")]
            public FeatureDetailsModel FeatureDetails { get; set; }

            [JsonProperty("url")]
            public Uri Url { get; set; }

            [JsonProperty("related_links")]
            public object RelatedLinks { get; set; }

            [JsonProperty("files")]
            public List<FileModel> Files { get; set; }

            [JsonProperty("subtitle_id")]
            public string SubtitleId { get; set; }

            [JsonProperty("moviehash_match")]
            public bool MovieHashMatch { get; set; }
        }

        [JsonObject]
        public class UploaderModel
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("rank")]
            public string Rank { get; set; }
        }

        [JsonObject]
        public class FeatureDetailsModel
        {
            [JsonProperty("feature_id")]
            public string FeatureId { get; set; }

            [JsonProperty("feature_type")]
            public string FeatureType { get; set; }

            [JsonProperty("year")]
            public int Year { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("imdb_id")]
            public string ImdbId { get; set; }

            [JsonProperty("tmdb_id")]
            public string TmdbId { get; set; }

            [JsonProperty("season_number")]
            public int SeasonNumber { get; set; }

            [JsonProperty("episode_number")]
            public int EpisodeNumber { get; set; }

            [JsonProperty("parent_imdb_id")]
            public string ParentImdbId { get; set; }

            [JsonProperty("parent_title")]
            public string ParentTitle { get; set; }

            [JsonProperty("parent_tmdb_id")]
            public string ParentTmdbId { get; set; }

            [JsonProperty("parent_feature_id")]
            public string ParentFeature_id { get; set; }
        }

        /// <summary>
        /// TODO: This model is sometimes returned as a single object or array.
        /// </summary>
        [JsonObject]
        public class RelatedLinkModel
        {
            [JsonProperty("label")]
            public string Label { get; set; }

            [JsonProperty("url")]
            public Uri Url { get; set; }

            [JsonProperty("img_url")]
            public Uri ImgUrl { get; set; }
        }

        [JsonObject]
        public class FileModel
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("cd_number")]
            public int CdNumber { get; set; }

            [JsonProperty("file_name")]
            public string FileName { get; set; }
        }
    }
}
