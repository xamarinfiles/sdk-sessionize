using System;
using SessionizeApi.Importer.Logger;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using SessionizeApi.Importer.Dtos;

namespace SessionizeApi.Importer.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [SuppressMessage("ReSharper",
        "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper",
        "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper",
        "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper",
        "UnusedAutoPropertyAccessor.Global")]
    public class QuestionAnswer : ILogFormattable
    {
        #region Constructor

        private QuestionAnswer(QuestionAnswerDto questionAnswerDto)
        {
            Id = questionAnswerDto.Id;
            AnswerText = questionAnswerDto.AnswerText;
        }

        public static QuestionAnswer Create(QuestionAnswerDto questionAnswerDto,
            LoggingService loggingService)
        {
            try
            {
                var item = new QuestionAnswer(questionAnswerDto);

                return item;
            }
            catch (Exception exception)
            {
                loggingService.LogExceptionRouter(exception);

                return null;
            }
        }

        #endregion

        #region Original and Replacement API Properties

        [JsonPropertyName("questionId")]
        public Id Id { get; set; }

        [JsonPropertyName("answerValue")]
        public string AnswerText { get; set; }

        #endregion

        #region Formatted Properties

        [JsonIgnore]
        public string DebuggerDisplay => $"{Id} - {AnswerText}";

        [JsonIgnore]
        public string LogDisplayShort => $"{Id,-3} - {AnswerText}";

        // TODO
        [JsonIgnore]
        public string LogDisplayLong => LogDisplayShort;

        #endregion
    }
}
