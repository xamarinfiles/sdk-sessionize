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
    public class Speaker : ILogFormattable
    {
        #region API Properties

        [JsonPropertyName("id")]
        public Id Id { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("bio")]
        public string Bio { get; set; }

        [JsonPropertyName("tagLine")]
        public string TagLine { get; set; }

        [JsonPropertyName("profilePicture")]
        public Uri ProfilePicture { get; set; }

        [JsonPropertyName("isTopSpeaker")]
        public bool IsTopSpeaker { get; set; }

        [JsonPropertyName("links")]
        public Link[] Links { get; set; }

        [JsonPropertyName("sessions")]
        public Id[] SessionIds { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonPropertyName("categoryItems")]
        public Id[] CategoryIds { get; set; }

        // TODO Make private?
        [JsonPropertyName("questionAnswers")]
        public QuestionAnswer[] QuestionAnswers { get; set; }

        #endregion

        #region Connective Properties

        [JsonPropertyName("categoryIdsAndNames ")]
        public IEnumerable<IdAndName> CategoryIdsAndNames { get; private set; }

        [JsonPropertyName("sessionIdsAndNames ")]
        public IEnumerable<IdAndName> SessionIdsAndNames { get; private set; }

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

        #endregion

        #region Formatting Methods

        // TODO Convert dictionary arguments to refs? [Affects LINQ expression]
        // TODO Pass QuestionDictionary
        internal void FormatDependentFields(
            IDictionary<Id, Session> sessionDictionary,
            IDictionary<Id, Item> categoryDictionary)
        {
            var sessionIdsAndNames =
                SessionIds
                    .Select(id =>
                        new IdAndName
                        {
                            Id = id,
                            Name = sessionDictionary[id].Title
                        })
                    .OrderBy(session => session.Id.Number)
                    .ToList();
            SessionIdsAndNames = sessionIdsAndNames;

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

            var idStr = Id + (IsTopSpeaker ? " (TOP)" : "");

            DebuggerDisplay =
                $"{idStr} - {FullName} - |Sessions| = {SessionIds.Length}";

            LogDisplayShort =
                $"{idStr,-10} - {FullName} - |Sessions| = {SessionIds.Length}";

            LogDisplayLong =
                CategoryIdsAndNames.Aggregate(LogDisplayShort,
                    (current, idAndName) =>
                        current
                        + $"{NewLine}{Indent}Category {idAndName.Id}: {idAndName.Name}");
            LogDisplayLong =
                SessionIdsAndNames.Aggregate(LogDisplayLong,
                    (current, idAndName) =>
                        current
                        + $"{NewLine}{Indent}Session {idAndName.Id}: {idAndName.Name}");
        }

        #endregion
    }
}
