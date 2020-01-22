using Newtonsoft.Json;
using System.Diagnostics;

namespace SessionizeApi.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Room
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sort")]
        public int Sort { get; set; }

        #region Debug

        public virtual string DebuggerDisplay => $"{Id} - {Sort} - {Name}";

        public virtual string LogDisplay => DebuggerDisplay;

        public override string ToString() => LogDisplay;

        #endregion
    }
}
