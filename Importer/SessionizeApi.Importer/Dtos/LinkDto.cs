using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace SessionizeApi.Importer.Dtos
{
    [SuppressMessage("ReSharper",
        "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper",
        "UnusedAutoPropertyAccessor.Global")]
    public class LinkDto
    {
        #region API Properties

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        [JsonPropertyName("linkType")]
        public string LinkType { get; set; }

        #endregion
    }
}
