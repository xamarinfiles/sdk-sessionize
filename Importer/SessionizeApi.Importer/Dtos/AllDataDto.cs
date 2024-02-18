using SessionizeApi.Importer.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SessionizeApi.Importer.Dtos
{
    //[DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [SuppressMessage("ReSharper",
        "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper",
        "UnusedAutoPropertyAccessor.Global")]
    public class AllDataDto // : ILogFormattable
    {
        #region API Properties

        [JsonPropertyName("sessions")]
        public SessionDto[] Sessions { get; set; }

        [JsonPropertyName("speakers")]
        public SpeakerDto[] Speakers { get; set; }

        [JsonPropertyName("questions")]
        public QuestionDto[] Questions { get; set; }

        // Sessionize "Categories" correspond to lists of single/multiple choice lists
        [JsonPropertyName("categories")]
        public ChoiceDto[] Choices { get; set; }

        [JsonPropertyName("rooms")]
        public ItemDto[] Rooms { get; set; }

        #endregion
    }
}
