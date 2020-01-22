using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SessionizeApi.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Session
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("startsAt")]
        public DateTimeOffset? StartsAt { get; set; }

        [JsonProperty("endsAt")]
        public DateTimeOffset? EndsAt { get; set; }

        [JsonProperty("isServiceSession")]
        public bool IsServiceSession { get; set; }

        [JsonProperty("isPlenumSession")]
        public bool IsPlenumSession { get; set; }

        [JsonProperty("speakers")]
        public Guid[] SpeakerIds { get; set; }

        [JsonProperty("categoryItems")]
        public int[] CategoryItems { get; set; }

        [JsonProperty("questionAnswers")]
        public QuestionAnswer[] QuestionAnswers { get; set; }

        [JsonProperty("roomId")]
        public int? RoomId { get; set; }

        #region Debug

        public virtual string DebuggerDisplay => $"{Id} - {Title}";

        public virtual string LogDisplay => $"{Id} - {Title,-80}";

        public IList<Speaker> Speakers { get; set; } = new List<Speaker>();

        // TODO NEXT Categories

        // TODO QuestionAnswers

        public override string ToString()
        {
            try
            {
                // TODO NEXT Add Categories
                // TODO Add Questions/QuestionsAnswers and Rooms
                var str = Speakers.Aggregate(LogDisplay,
                    (current, speaker) =>
                        current + $"\n\t\t\t\t{speaker.LogDisplay}");

                return str;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);

                return $"{Id} - {Title,-70}";
            }
        }

        #endregion
    }
}
