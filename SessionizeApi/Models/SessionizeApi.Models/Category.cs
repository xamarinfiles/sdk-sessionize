using Newtonsoft.Json;
using System.Diagnostics;

namespace SessionizeApi.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }

        [JsonProperty("sort")]
        public int Sort { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        #region Debug

        public virtual string DebuggerDisplay => $"{Id} - {Sort} - {Title}";

        public virtual string LogDisplay => DebuggerDisplay;

        // TODO Add Items
        public override string ToString() => LogDisplay;

        #endregion
    }
}
