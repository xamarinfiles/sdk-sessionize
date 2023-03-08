using SessionizeApi.Importer.Dtos;
using SessionizeApi.Importer.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using static SessionizeApi.Importer.Serialization.Configurations;
using static System.Text.Json.JsonSerializer;

namespace SessionizeApi.Importer.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class Event : ILogFormattable
    {
        #region Constructors

        private Event(AllDataDto allDataDto, LoggingService loggingService,
            string eventSource)
        {
            Sessions = allDataDto.Sessions
                .Select(sessionDto =>
                    Session.Create(sessionDto, loggingService))
                .OrderBy(session => session.Title)
                .ToArray();
            Speakers = allDataDto.Speakers
                .Select(speakerDto =>
                    Speaker.Create(speakerDto, loggingService))
                .OrderBy(speaker => speaker.FullName)
                .ToArray();
            Questions = allDataDto.Questions
                .Select(questionDto =>
                    Question.Create(questionDto, loggingService))
                .OrderBy(question => question.Sort)
                .ToArray();
            Categories = allDataDto.Categories
                .Select( categoryDto =>
                    Category.Create(categoryDto, loggingService))
                .OrderBy(category => category.Sort)
                .ToArray();
            Rooms = allDataDto.Rooms
                .Select( roomDto =>
                    Item.Create(roomDto, loggingService))
                .OrderBy(room => room.Sort)
                .ToArray();

            PopulateReferenceDictionaries();
            FormatReferenceFields(loggingService);

            FormatLogFields(eventSource, loggingService);
        }

        public static Event Create(string allDataJson,
            LoggingService loggingService, string eventSource = null)
        {
            try
            {
                var allData = FromJson(allDataJson, loggingService, eventSource);

                var @event = new Event(allData, loggingService, eventSource);

                return @event;
            }
            catch (Exception exception)
            {
                loggingService.LogExceptionRouter(exception);

                return null;
            }
        }

        #endregion

        // TODO Keep as arrays?
        #region Replacement API Properties

        [JsonPropertyName("sessions")]
        public Session[] Sessions { get; set; }

        [JsonPropertyName("speakers")]
        public Speaker[] Speakers { get; set; }

        [JsonPropertyName("questions")]
        public Question[] Questions { get; set; }

        [JsonPropertyName("categories")]
        public Category[] Categories { get; set; }

        [JsonPropertyName("rooms")]
        public Item[] Rooms { get; set; }

        #endregion

        #region Reference Properties

        [JsonIgnore]
        public IDictionary<Id, Session> SessionDictionary { get; set; }

        [JsonIgnore]
        public IDictionary<Id, Speaker> SpeakerDictionary { get; set; }

        [JsonIgnore]
        public IDictionary<Id, Item> CategoryDictionary { get; set; }

        #endregion

        #region Formatted Properties

        [JsonIgnore]
        public string DebuggerDisplay { get; protected set; }

        [JsonIgnore]
        public string LogDisplayShort { get; protected set; }

        [JsonIgnore]
        public string LogDisplayLong { get; protected set; }

        #endregion

        #region Load Methods

        private static AllDataDto FromJson(string json,
            LoggingService loggingService, string eventSource)
        {
            try
            {
                if (Deserialize<AllDataDto>(json, ParseJsonOptions) is
                    { } allData)
                {
                    return allData;
                }
            }
            catch (Exception exception)
            {
                loggingService.LogExceptionRouter(exception);
            }

            return null;
        }

        #endregion

        #region Reference Methods

        private void PopulateReferenceDictionaries()
        {
            SessionDictionary =
                Sessions.ToDictionary(session => session.Id);

            SpeakerDictionary =
                Speakers.ToDictionary(speaker => speaker.Id);

            CategoryDictionary = new Dictionary<Id, Item>();
            foreach (var (item, itemId) in
                     Categories
                         .SelectMany(
                             category =>
                                 category.Items.Select(item => (item, item.Id))))
            {
                CategoryDictionary[itemId] = item;
            }
        }

        private void FormatReferenceFields(LoggingService loggingService)
        {
            foreach (var session in Sessions)
            {
                session.FormatReferenceFields(SpeakerDictionary,
                    CategoryDictionary, loggingService);
            }

            foreach (var speaker in Speakers)
            {
                speaker.FormatReferenceFields(SessionDictionary,
                    CategoryDictionary, loggingService);
            }
        }

        #endregion

        #region Formatting Methods

        private void FormatLogFields(string eventSource,
            LoggingService loggingService)
        {
            try
            {
                DebuggerDisplay = $"Source = {eventSource ?? string.Empty}";
                LogDisplayShort =
                    DebuggerDisplay +
                    $" - |Sessions| = {Sessions.Length} - |Speakers| = {Speakers.Length}";
                // TEMP
                LogDisplayLong = LogDisplayShort;
            }
            catch (Exception exception)
            {
                loggingService.LogExceptionRouter(exception);
            }
        }

        #endregion
    }
}
