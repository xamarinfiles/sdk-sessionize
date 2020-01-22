using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace SessionizeApi.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Link
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }

        [JsonProperty("linkType")]
        public string LinkType { get; set; }

        #region Debug

        public virtual string DebuggerDisplay => $"{Title}: {Url}";

        public virtual string LogDisplay => DebuggerDisplay;

        public override string ToString() => LogDisplay;

        #endregion
    }
}
