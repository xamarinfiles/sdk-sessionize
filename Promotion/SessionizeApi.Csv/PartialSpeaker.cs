using System;
using System.Text.Json.Serialization;

namespace SessionizeApi.Csv
{
    internal class PartialSpeaker
    {
        [JsonPropertyName("SpeakerName")]
        public string FullName { get; set; }

        // Not exported by Sessionize
        //[JsonPropertyName("SpeakerEmail")]
        //public string EmailAddress { get; set; }

        [JsonPropertyName("ProfilePicture")]
        public Uri ProfilePicture { get; set; }

        [JsonPropertyName("LinkedInLink")]
        public Uri LinkedInLink { get; set; }

        [JsonPropertyName("TwitterLink")]
        public Uri TwitterLink { get; set; }

        [JsonPropertyName("TwitterHandle")]
        public string TwitterHandle { get; set; }

        [JsonPropertyName("MastodonLink")]
        public Uri MastodonLink { get; set; }

        [JsonPropertyName("MastodonHandle")]
        public string MastodonHandle { get; set; }

        [JsonPropertyName("SessionTitle")]
        public string SessionTitle { get; set;}

        [JsonPropertyName("SessionLink")]
        public Uri SessionLink { get; set; }
    }
}
