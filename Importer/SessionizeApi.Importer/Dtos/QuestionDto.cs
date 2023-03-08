using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SessionizeApi.Importer.Dtos
{
    [SuppressMessage("ReSharper",
        "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper",
        "UnusedAutoPropertyAccessor.Global")]
    public class QuestionDto
    {
        #region API Properties

        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("question")]
        public string QuestionText { get; set; }

        [JsonPropertyName("questionType")]
        public string QuestionType { get; set; }

        [JsonPropertyName("sort")]
        public int Sort { get; set; }

        #endregion
    }
}
