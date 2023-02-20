using SessionizeApi.Importer.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using static SessionizeApi.Importer.Constants.Characters;

namespace SessionizeApi.Importer.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class Session : ILogFormattable
    {
        #region API Properties

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

        [JsonPropertyName("speakers")]
        public Id[] SpeakerIds { get; set; }

        [JsonPropertyName("categoryItems")]
        public Id[] CategoryIds { get; set; }

        // TODO Make private?
        [JsonPropertyName("questionAnswers")]
        public QuestionAnswer[] QuestionAnswers { get; set; }

        // TODO Check format when have data
        [JsonPropertyName("liveUrl")]
        public string LiveUrl { get; set; }

        // TODO Check format when have data
        [JsonPropertyName("recordingUrl")]
        public string RecordingUrl { get; set; }

        #endregion

        #region Connective Properties

        [JsonPropertyName("categoryIdsAndNames ")]
        public IEnumerable<IdAndName> CategoryIdsAndNames { get; private set; }

        [JsonPropertyName("speakerIdsAndNames")]
        public IEnumerable<IdAndName> SpeakerIdsAndNames { get; private set; }

        // TODO
        //public IEnumerable<IdAndFieldAndValue> QuestionsIdsAndAnswers { get; private set; }

        #endregion

        #region Formatted Properties

        [JsonIgnore]
        public string DebuggerDisplay { get; private set; }

        [JsonIgnore]
        public string LogDisplayShort { get; private set; }

        [JsonIgnore]
        public string LogDisplayLong { get; private set; }

         [JsonIgnore]
        public string SpeakerNames { get; private set; }

        #endregion

        #region Formatting Methods

        // TODO Convert dictionary arguments to refs? [Affects LINQ expression]
        // TODO Pass QuestionDictionary
        internal void FormatDependentFields(
            IDictionary<Id, Speaker> speakerDictionary,
            IDictionary<Id, Item> categoryDictionary)
        {
            var speakerIdsAndNames =
                SpeakerIds
                    .Select(id =>
                        new IdAndName
                        {
                            Id = id,
                            Name = speakerDictionary[id].FullName
                        })
                    .OrderBy(speaker => speaker.Name)
                    .ToList();
            SpeakerIdsAndNames = speakerIdsAndNames;

            var categoryIdsAndNames =
                CategoryIds
                    .Select(id =>
                        new IdAndName
                        {
                            Id = id,
                            Name = categoryDictionary[id].Name
                        })
                    .OrderBy(category => category.Name)
                    .ToList();
            CategoryIdsAndNames = categoryIdsAndNames;

            // TODO Merge QuestionsIdsAndAnswers from question dictionary and answers

            SpeakerNames =
                string.Join(", ",
                    SpeakerIdsAndNames.Select(tuple => tuple.Name));

            var idStr = IsServiceSession ? "SERVICE" : Id.ToString();

            DebuggerDisplay =
                $"{idStr} - Speakers: {SpeakerNames} - {Title}";

            LogDisplayShort =
                $"{idStr,-10} - Speakers: {SpeakerNames,-40} - {Title}";

            LogDisplayLong = $"{idStr,-10} - {Title}";
            LogDisplayLong =
                CategoryIdsAndNames.Aggregate(LogDisplayLong,
                    (current, idAndName) =>
                        current
                        + $"{NewLine}{Indent}Category {idAndName.Id}: {idAndName.Name}");
            LogDisplayLong =
                SpeakerIdsAndNames.Aggregate(LogDisplayLong,
                    (current, idAndName) =>
                        current
                        + $"{NewLine}{Indent}Speaker {idAndName.Id}: {idAndName.Name}");
        }

        #endregion
    }
}
