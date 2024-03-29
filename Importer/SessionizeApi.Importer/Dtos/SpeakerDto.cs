using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SessionizeApi.Importer.Dtos
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class SpeakerDto
    {
        #region API Properties

        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("bio")]
        public string Bio { get; set; }

        [JsonPropertyName("tagLine")]
        public string TagLine { get; set; }

        [JsonPropertyName("profilePicture")]
        public Uri ProfilePicture { get; set; }

        // TODO
        //[JsonPropertyName("isTopSpeaker")]
        //public bool IsTopSpeaker { get; set; }

        [JsonPropertyName("links")]
        public LinkDto[] Links { get; set; }

        [JsonPropertyName("sessions")]
        public uint[] SessionIds { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonPropertyName("categoryItems")]
        public uint[] ChoiceIds { get; set; }

        [JsonPropertyName("questionAnswers")]
        public QuestionAnswerDto[] QuestionAnswers { get; set; }

        #endregion
    }
}
