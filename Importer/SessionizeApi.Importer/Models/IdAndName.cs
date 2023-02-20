using SessionizeApi.Importer.Logger;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace SessionizeApi.Importer.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class IdAndName : ILogFormattable
    {
        #region API Properties

        [JsonPropertyName("id")]
        public Id Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        #endregion

        #region Formatted Properties

        [JsonIgnore]
        public string DebuggerDisplay => $"{Id} - {Name}";

        [JsonIgnore]
        public string LogDisplayShort => $"{Id, -3} - {Name}";

        [JsonIgnore]
        public string LogDisplayLong => LogDisplayShort;

        #endregion

        #region Formatting Methods

        // TODO
        public override string ToString() => DebuggerDisplay;

        #endregion
    }
}
