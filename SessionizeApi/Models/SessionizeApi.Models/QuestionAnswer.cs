using Newtonsoft.Json;
using System.Diagnostics;

namespace SessionizeApi.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class QuestionAnswer
    {
        [JsonProperty("questionId")]
        public int Id { get; set; }

        [JsonProperty("answerValue")]
        public string AnswerText { get; set; }

        #region Debug

        public virtual string DebuggerDisplay => $"{Id} - {AnswerText}";

        public virtual string LogDisplay => DebuggerDisplay;

        public override string ToString() => LogDisplay;

        #endregion
    }
}
