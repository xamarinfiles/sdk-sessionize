using SessionizeApi.Importer.Dtos;
using SessionizeApi.Importer.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using XamarinFiles.FancyLogger;
using static SessionizeApi.Importer.Constants.Characters;

namespace SessionizeApi.Importer.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Session : ILogFormattable
    {
        #region Constructor

        private Session(SessionDto oldSessionDto,
            IFancyLogger fancyLogger)
        {
            try
            {
                // API Properties

                Id = oldSessionDto.Id;
                Title = oldSessionDto.Title;
                Description = oldSessionDto.Description;
                StartsAt = oldSessionDto.StartsAt;
                EndsAt = oldSessionDto.EndsAt;
                IsServiceSession = oldSessionDto.IsServiceSession;
                IsPlenumSession = oldSessionDto.IsPlenumSession;
                QuestionAnswers = oldSessionDto.QuestionAnswers
                    .Select(answerDto =>
                        QuestionAnswer.Create(answerDto,
                            fancyLogger))
                    .ToArray();
                RoomId = oldSessionDto.RoomId;
                LiveUrl = oldSessionDto.LiveUrl;
                RecordingUrl = oldSessionDto.RecordingUrl;

                //Reference Properties

                SpeakerIds = oldSessionDto.SpeakerIds
                    .Select(speakerIdGuid => (Id)speakerIdGuid)
                    .ToArray();
                ChoiceIds = oldSessionDto.ChoiceIds
                    .Select(choiceIdUint => (Id)choiceIdUint)
                    .ToArray();
            }
            catch (Exception exception)
            {
                fancyLogger.LogException(exception);
            }
        }

        public static Session Create(SessionDto oldSessionDto,
            IFancyLogger fancyLogger)
        {
            try
            {
                var session = new Session(oldSessionDto, fancyLogger);

                // TEMP
                fancyLogger.LogObject<Session>(session);
                fancyLogger.LogScalar($"Session {session.Id.Guid}", session.ToString());

                return session;
            }
            catch (Exception exception)
            {
                fancyLogger.LogException(exception);

                return null;
            }
        }

        #endregion

        #region Original and Replacement API Properties

        [JsonPropertyName("id")]
        public Id Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("startsAt")]
        public DateTimeOffset? StartsAt { get; set; }

        [JsonPropertyName("endsAt")]
        public DateTimeOffset? EndsAt { get; set; }

        [JsonPropertyName("isServiceSession")]
        public bool IsServiceSession { get; set; }

        [JsonPropertyName("isPlenumSession")]
        public bool IsPlenumSession { get; set; }

        [JsonPropertyName("speakerIds")]
        public Id[] SpeakerIds { get; set; }

        [JsonPropertyName("categoryIds")]
        public Id[] ChoiceIds { get; set; }

        [JsonPropertyName("questionAnswers")]
        public QuestionAnswer[] QuestionAnswers { get; set; }

        [JsonPropertyName("roomId")]
        internal Id? RoomId { get; set; }

        // TODO Check format and decide conversion when have data
        [JsonPropertyName("liveUrl")]
        public string LiveUrl { get; set; }

        // TODO Check format and decide conversion when have data
        [JsonPropertyName("recordingUrl")]
        public string RecordingUrl { get; set; }

        #endregion

        #region Reference Properties

        [JsonPropertyName("categoryReferences ")]
        public IEnumerable<Item> ChoiceReferences { get; private set; }

        // TODO
        [JsonPropertyName("questionReferences")]
        public IEnumerable<Item> QuestionsReferences { get; private set; }

        [JsonPropertyName("speakerReferences")]
        public IEnumerable<Item> SpeakerReferences { get; private set; }

        #endregion

        #region Formatted Properties

        [JsonIgnore]
        public string DebuggerDisplay { get; private set; }

        [JsonIgnore]
        public string LogDisplayShort { get; private set; }

        [JsonIgnore]
        public string LogDisplayLong { get; private set; }

        // Used in Trello exporter also
        [JsonIgnore]
        public string SpeakerNames { get; private set; }

        #endregion

        #region Formatting Methods

        // TODO Convert dictionary arguments to refs? [Affects LINQ expression]
        // TODO Pass QuestionDictionary
        internal void FormatReferenceFields(
            IDictionary<Id, Speaker> speakerDictionary,
            IDictionary<Id, Item> choiceDictionary,
            IFancyLogger fancyLogger)
        {
            var speakerReferences = SpeakerIds
                // Dereference Speaker Id to get Full Name
                .Select(id =>
                    (id, speakerDictionary[id].FullName))
                // Sort alphabetically by Speaker's Full Name
                .OrderBy(idAndName =>
                    idAndName.FullName)
                // Project into Item in same alphabetical order
                .Select((idAndName, index) =>
                    Item.Create(idAndName.id, idAndName.FullName, (uint)index,
                        fancyLogger))
                .ToList();
            SpeakerReferences = speakerReferences;

            var choiceReferences = ChoiceIds
                // Choices are already Items => Pull directly from dictionary
                .Select(id => choiceDictionary[id])
                // Sort by Choice name in case added out of alphabetical order
                .OrderBy(choice => choice.Name)
                .ToList();
            ChoiceReferences = choiceReferences;

            // TODO Pull Question Ids and Names from dictionary => Item list

            SpeakerNames =
                string.Join(", ",
                    SpeakerReferences.Select(item => item.Name));

            var idStr = IsServiceSession ? "SERVICE" : Id.ToString();

            DebuggerDisplay =
                $"{idStr} - Speakers: {SpeakerNames} - {Title}";

            LogDisplayShort =
                $"{idStr,-7} - Speakers: {SpeakerNames,-30} - {Title}";

            LogDisplayLong = $"{idStr,-10} - {Title}";
            LogDisplayLong =
                ChoiceReferences.Aggregate(LogDisplayLong,
                    (current, item) =>
                        current
                        + $"{NewLine}{Indent}Choice {item.Id}: {item.Name}");
            LogDisplayLong =
                SpeakerReferences.Aggregate(LogDisplayLong,
                    (current, item) =>
                        current
                        + $"{NewLine}{Indent}Speaker {item.Id}: {item.Name}");
        }

        #endregion
    }
}
