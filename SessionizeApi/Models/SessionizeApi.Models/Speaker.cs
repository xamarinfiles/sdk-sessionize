using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SessionizeApi.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Speaker
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("tagLine")]
        public string TagLine { get; set; }

        [JsonProperty("profilePicture")]
        public Uri ProfilePicture { get; set; }

        [JsonProperty("isTopSpeaker")]
        public bool IsTopSpeaker { get; set; }

        [JsonProperty("links")]
        public Link[] Links { get; set; }

        [JsonProperty("sessions")]
        public int[] SessionIds { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("categoryItems")]
        public int[] CategoryItems { get; set; }

        [JsonProperty("questionAnswers")]
        public QuestionAnswer[] QuestionAnswers { get; set; }

        #region Debug

        public virtual string DebuggerDisplay =>
            $"{FullName} - {SessionIds.Length} - {Id}";

        public virtual string LogDisplay =>
            $"{FullName,-20} - {SessionIds.Length,-2} - {Id}";

        public IList<Session> Sessions { get; set; } = new List<Session>();

        // TODO Add lists of Categories, Links, and QuestionAnswers

        public override string ToString()
        {
            try
            {
                // TODO Add Categories, Links, and Questions/QuestionsAnswers
                var str = Sessions.Aggregate(LogDisplay,
                    (current, session) =>
                        current + $"\n\t\t\t\t{session.LogDisplay}");

                return str;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);

                return $"{FullName,-20} - {SessionIds.Length,-2} - {Id}";
            }
        }

        #endregion
    }
}
