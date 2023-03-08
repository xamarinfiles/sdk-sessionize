using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SessionizeApi.Importer.Dtos
{
    [SuppressMessage("ReSharper",
        "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper",
        "UnusedAutoPropertyAccessor.Global")]
    public class ItemDto
    {
        #region API Properties

        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sort")]
        public uint Sort { get; set; }

        #endregion

    }
}
