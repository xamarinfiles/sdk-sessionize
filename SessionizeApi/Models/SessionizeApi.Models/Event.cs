using Newtonsoft.Json;
using SessionizeApi.Models.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SessionizeApi.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Event
    {
        public static Event FromJson(string json, string eventSource = null)
        {
            try
            {

                var @event =
                    JsonConvert.DeserializeObject<Event>(json, EventConverter.Settings);
                @event.DebuggerDisplay = eventSource ?? string.Empty;

                return @event;
            }
            catch
            {
                return null;
            }
        }

        [JsonProperty("categories")]
        public Category[] Categories { get; set; }

        [JsonProperty("questions")]
        public Question[] Questions { get; set; }

        [JsonProperty("rooms")]
        public Room[] Rooms { get; set; }

        [JsonProperty("sessions")]
        public Session[] Sessions { get; set; }

        [JsonProperty("speakers")]
        public Speaker[] Speakers { get; set; }

        #region Debug

        public virtual string DebuggerDisplay { get; set; }

        public virtual string LogDisplay => DebuggerDisplay;

        public override string ToString() => LogDisplay;

        public IDictionary<int, Item> CategoryDictionary { get; set; }

        public IDictionary<int, Session> SessionDictionary { get; set; }

        public IDictionary<Guid, Speaker> SpeakerDictionary { get; set; }

        #endregion
    }
}
