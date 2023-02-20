using SessionizeApi.Importer.Logger;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SessionizeApi.Importer.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Question : ILogFormattable
    {
        #region API Properties

        [JsonPropertyName("id")]
        public Id Id { get; set; }

        [JsonPropertyName("question")]
        public string QuestionText { get; set; }

        // TODO Convert to enum
        [JsonPropertyName("questionType")]
        public string QuestionType { get; set; }

        [JsonPropertyName("sort")]
        public int Sort { get; set; }

        #endregion

        #region Formatted Properties

        [JsonIgnore]
        public string DebuggerDisplay => $"{Id} - {Sort} - {QuestionText}";

        [JsonIgnore]
        public string LogDisplayShort => $"{Id,-3} - {Sort,-3} - {QuestionText}";

        // TODO
        [JsonIgnore]
        public string LogDisplayLong => LogDisplayShort;

        // TODO ToString?

        #endregion

    }
}
