using Newtonsoft.Json;
using System.Diagnostics;

namespace SessionizeApi.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Question
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("question")]
        public string QuestionText { get; set; }

        [JsonProperty("questionType")]
        public string QuestionType { get; set; }

        [JsonProperty("sort")]
        public int Sort { get; set; }

        #region Debug

        public virtual string DebuggerDisplay => $"{Id} - {Sort} - {QuestionText}";

        public virtual string LogDisplay => DebuggerDisplay;

        public override string ToString() => LogDisplay;

        #endregion

    }
}
