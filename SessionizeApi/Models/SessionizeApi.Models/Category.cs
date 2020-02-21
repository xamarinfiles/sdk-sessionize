using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;

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

        public override string ToString()
        {
            try
            {
                var str = LogDisplay;
                str = Items.Aggregate(str,
                    (current, item) => current + $"\n\t\t\t\t{item.LogDisplay}");

                return str;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);

                return DebuggerDisplay;
            }
        }

        #endregion
    }
}
