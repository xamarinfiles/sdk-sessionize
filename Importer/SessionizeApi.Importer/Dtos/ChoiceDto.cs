using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SessionizeApi.Importer.Dtos
{
    [SuppressMessage("ReSharper",
        "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper",
        "UnusedAutoPropertyAccessor.Global")]
    public class ChoiceDto
    {
        #region API Properties

        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("items")]
        public ItemDto[] Items { get; set; }

        [JsonPropertyName("sort")]
        public uint Sort { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        #endregion
    }
}
