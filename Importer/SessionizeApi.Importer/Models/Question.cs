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
    public class Question : ILogFormattable
    {
        #region Constructor

        private Question(QuestionDto questionDto)
        {
            Id = questionDto.Id;
            QuestionText = questionDto.QuestionText;
            QuestionType = questionDto.QuestionType;
            Sort = questionDto.Sort;
        }

        public static Question Create(QuestionDto questionDto,
            LoggingService loggingService)
        {
            try
            {
                var item = new Question(questionDto);

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

        [JsonPropertyName("id")]
        public Id Id { get; set; }

        [JsonPropertyName("question")]
        public string QuestionText { get; set; }

        // TODO Convert to enum
        [JsonPropertyName("questionType")]
        public string QuestionType { get; set; }

        [JsonPropertyName("sort")]
        public int Sort { get; set; }

        #endregion

        #region Formatted Properties

        [JsonIgnore]
        public string DebuggerDisplay => $"{Id} - {Sort} - {QuestionText}";

        [JsonIgnore]
        public string LogDisplayShort => $"{Id,-3} - {Sort,-3} - {QuestionText}";

        // TODO
        [JsonIgnore]
        public string LogDisplayLong => LogDisplayShort;

        #endregion
    }
}
