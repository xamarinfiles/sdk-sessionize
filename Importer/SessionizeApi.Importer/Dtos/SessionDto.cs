using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SessionizeApi.Importer.Dtos
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class SessionDto
    {
        #region Original API Properties

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("startsAt")]
        public DateTimeOffset? StartsAt { get; set; }

        [JsonPropertyName("endsAt")]
        public DateTimeOffset? EndsAt { get; set; }

        [JsonPropertyName("isServiceSession")]
        public bool IsServiceSession { get; set; }

        [JsonPropertyName("isPlenumSession")]
        public bool IsPlenumSession { get; set; }

        [JsonPropertyName("speakers")]
        public Guid[] SpeakerIds { get; set; }

        [JsonPropertyName("categoryItems")]
        public uint[] CategoryIds { get; set; }

        [JsonPropertyName("questionAnswers")]
        public QuestionAnswerDto[] QuestionAnswers { get; set; }

        [JsonPropertyName("roomId")]
        public uint RoomId { get; set; }

        // TODO Check format and decide conversion when have data
        [JsonPropertyName("liveUrl")]
        public string LiveUrl { get; set; }

        // TODO Check format and decide conversion when have data
        [JsonPropertyName("recordingUrl")]
        public string RecordingUrl { get; set; }

        #endregion
    }
}
