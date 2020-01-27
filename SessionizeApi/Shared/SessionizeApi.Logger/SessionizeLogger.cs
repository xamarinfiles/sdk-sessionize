using FancyLogger;
using SessionizeApi.Models;
using System;
using System.Diagnostics;

namespace SessionizeApi.Logger
{
    internal class SessionizeLogger
    {
        #region Services

        private FancyLoggerService LoggingService { get; set; }

        #endregion

        #region Constructor

        // TODO Fix CodeRush formatting
        internal SessionizeLogger(FancyLoggerService loggingService) => LoggingService = loggingService;

        #endregion

        #region Debugging / Discovery / Verification

        [Conditional("DEBUG")]
        internal void PrintEvent(Event @event)
        {
            if (@event == null)
                return;

            try
            {
                var header = string.IsNullOrEmpty(@event.DebuggerDisplay)
                    ? "All Data"
                    : @event.DebuggerDisplay;
                LoggingService.WriteHeader(header);
                PrintSpeakers(@event.Speakers);
                PrintSessions(@event.Sessions);
                PrintQuestions(@event.Questions);
                PrintCategories(@event.Categories);
                PrintRooms(@event.Rooms);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        #endregion

        #region Categories

        [Conditional("DEBUG")]
        private void PrintCategories(Category[] categories)
        {
            if (!(categories?.Length > 1))
                return;

            try
            {
                LoggingService.WriteSubheader("Categories");
                LoggingService.WriteList(categories);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        #endregion

        #region Questions

        [Conditional("DEBUG")]
        private void PrintQuestions(Question[] questions)
        {
            if (!(questions?.Length > 1))
                return;

            try
            {
                LoggingService.WriteSubheader("Questions");
                LoggingService.WriteList(questions);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        #endregion

        #region Rooms

        [Conditional("DEBUG")]
        private void PrintRooms(Room[] rooms)
        {
            if (!(rooms?.Length > 1))
                return;

            try
            {
                LoggingService.WriteSubheader("Rooms");
                LoggingService.WriteList(rooms);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        #endregion

        #region Speakers

        [Conditional("DEBUG")]
        private void PrintSpeakers(Speaker[] speakers)
        {
            if (!(speakers?.Length > 1))
                return;

            try
            {
                LoggingService.WriteSubheader("Speakers");
                LoggingService.WriteList(speakers);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        #endregion

        #region Sessions

        [Conditional("DEBUG")]
        private void PrintSessions(Session[] sessions)
        {
            if (!(sessions?.Length > 1))
                return;

            try
            {
                LoggingService.WriteSubheader("Sessions");
                LoggingService.WriteList(sessions);

            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        #endregion
    }
}
