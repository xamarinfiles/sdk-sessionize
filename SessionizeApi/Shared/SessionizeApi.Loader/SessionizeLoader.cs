using FancyLogger;
using SessionizeApi.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace SessionizeApi.Loader
{
    internal static class SessionizeLoader
    {
        #region Services

        private static FancyLoggerService LoggingService { get; set; }

        #endregion

        internal SessionizeLoader(FancyLoggerService loggingService) => LoggingService = loggingService;

        #region Event Loader

        internal Event LoadEvent(string eventJsonFileName)
        {
            try
            {
                var allDataJson = GetContentTextFile(eventJsonFileName);

                if (allDataJson == null)
                    return null;

                var @event = Event.FromJson(allDataJson);
                @event.DebuggerDisplay = eventJsonFileName;
                PopulateDependencies(ref @event);

                return @event;
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);

                return null;
            }
        }

        #endregion

        #region File Handling

        private static string GetContentTextFile(string filename)
        {
            try
            {
                return File.ReadAllText(filename);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);

                return null;
            }
        }

        #endregion

        #region Dependent Objects

        [Conditional("DEBUG")]
        private static void PopulateDependencies(ref Event @event)
        {
            PopulateIndexDictionaries(ref @event);
            PopulateSessionDependencies(ref @event);
            PopulateSpeakerDependencies(ref @event);
            // TODO NEXT Add Categories
            // TODO Add Questions
        }

        [Conditional("DEBUG")]
        private static void PopulateIndexDictionaries(ref Event @event)
        {
            @event.SessionDictionary =
                @event.Sessions.ToDictionary(s => s.Id);
            @event.SpeakerDictionary =
                @event.Speakers.ToDictionary(s => s.Id);
        }

        [Conditional("DEBUG")]
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

        [Conditional("DEBUG")]
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

        #endregion
    }
}
