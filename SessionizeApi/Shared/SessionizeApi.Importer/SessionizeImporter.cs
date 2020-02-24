using FancyLogger;
using SessionizeApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SessionizeApi.Importer
{
    internal class SessionizeImporter
    {
        #region Services

        private static FancyLoggerService LoggingService { get; set; }

        #endregion

        #region Constructor

        internal SessionizeImporter(FancyLoggerService loggingService) =>
            LoggingService = loggingService;

        #endregion

        #region All Data (Full Event) Importer

        public Event ImportAllDataFromFile(string jsonFilename,
            CustomSort customSort = CustomSort.Unsorted, bool includeDependencies = false)
        {
            var allDataJson = GetContentTextFile(jsonFilename);

            var @event =
                ImportAllDataFromJson(allDataJson, customSort, includeDependencies,
                    jsonFilename);

            return @event;
        }

        public Event ImportAllDataFromUri(Uri jsonUri,
            CustomSort customSort = CustomSort.Unsorted, bool includeDependencies = false)
        {
            try
            {
                Event @event = null;

                using (var client = new System.Net.WebClient())
                {
                    var allDataJson = client.DownloadString(jsonUri);

                    var eventSource = jsonUri.AbsoluteUri;

                    @event = ImportAllDataFromJson(allDataJson, customSort,
                        includeDependencies, eventSource);
                }

                return @event;
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);

                return null;
            }
        }

        public Event ImportAllDataFromJson(string allDataJson,
            CustomSort customSort = CustomSort.Unsorted, bool includeDependencies = false,
            string eventSource = null)
        {
            try
            {
                if (allDataJson == null)
                    return null;

                var @event = Event.FromJson(allDataJson, eventSource);
                if (includeDependencies)
                    PopulateDependencies(ref @event);

                var transformedEvent = TransformEvent(@event, customSort);

                if (transformedEvent != null)
                {
                    return transformedEvent;
                }

                LoggingService.WriteWarning(
                    $"Event transformation failed {customSort} - Using default sort");

                return @event;
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);

                return null;
            }
        }

        protected virtual Event TransformEvent(Event @event, CustomSort sort) => @event;

        #endregion

        #region File Handling

        private static string GetContentTextFile(string path)
        {
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);

                return null;
            }
        }

        #endregion

        #region Dependent Objects

        private static void PopulateDependencies(ref Event @event)
        {
            PopulateIndexDictionaries(ref @event);
            PopulateSessionDependencies(ref @event);
            PopulateSpeakerDependencies(ref @event);
            PopulateCategoryDependencies(ref @event);
            // TODO Add Questions
        }

        private static void PopulateIndexDictionaries(ref Event @event)
        {
            // Only include speaker sessions and not special sessions
            @event.SessionDictionary =
                @event.Sessions.Where(s => s.Id.Integer != null)
                    .ToDictionary(s => (int) s.Id.Integer);

            @event.SpeakerDictionary =
                @event.Speakers.ToDictionary(s => s.Id);

            @event.CategoryDictionary = new Dictionary<int, Item>();
            foreach (var (item, itemId) in
                @event.Categories.SelectMany(
                    category => category.Items.Select(
                        item => (item, item.Id))))
            {
                @event.CategoryDictionary[itemId] = item;
            }
        }

        private static void PopulateSessionDependencies(ref Event @event)
        {
            foreach (var (session, speakerId) in
                @event.Sessions.SelectMany(
                    session =>
                        session.SpeakerIds.Select(speakerId => (session, speakerId))))
            {
                session.Speakers.Add(@event.SpeakerDictionary[speakerId]);
            }
        }

        private static void PopulateSpeakerDependencies(ref Event @event)
        {
            foreach (var (speaker, sessionId) in
                @event.Speakers.SelectMany(
                    speaker =>
                        speaker.SessionIds.Select(sessionId => (speaker, sessionId))))
            {
                speaker.Sessions.Add(@event.SessionDictionary[sessionId]);
            }
        }

        private static void PopulateCategoryDependencies(ref Event @event)
        {
            foreach (var (session, itemId) in
                @event.Sessions.SelectMany(
                    session => session.CategoryIds.Select(
                        itemId => (session, itemId)).OrderBy(itemId => itemId)))
            {
                session.CategoryItems.Add(@event.CategoryDictionary[itemId]);
            }

            // TODO Speaker Categories
        }

        #endregion
    }
}
