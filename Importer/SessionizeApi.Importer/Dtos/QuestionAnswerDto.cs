using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SessionizeApi.Importer.Dtos
{
    [SuppressMessage("ReSharper",
        "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper",
        "UnusedAutoPropertyAccessor.Global")]
    public class QuestionAnswerDto
    {
        #region API Properties

        [JsonPropertyName("questionId")]
        public uint Id { get; set; }

        [JsonPropertyName("answerValue")]
        public string AnswerText { get; set; }

        #endregion
    }
}
