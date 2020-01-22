using FancyLogger;
using SessionizeApi.Models;
using System;

namespace SessionizeApi.Logger
{
    internal static class SessionizeLogger
    {
        #region Services

        public static FancyLoggerService LoggingService { get; set; }

        #endregion

        #region All Data

        internal static void PrintEvent(Event @event)
        {
            if (@event == null)
                return;

            try
            {
                var header = string.IsNullOrEmpty(@event.DebuggerDisplay)
                    ? "All Data"
                    : @event.DebuggerDisplay;
                LoggingService?.WriteHeader(header);
                PrintSpeakers(@event.Speakers);
                PrintSessions(@event.Sessions);
                PrintQuestions(@event.Questions);
                PrintCategories(@event.Categories);
                PrintRooms(@event.Rooms);
            }
            catch (Exception exception)
            {
                LoggingService?.WriteException(exception);
            }
        }

        #endregion

        #region Categories

        private static void PrintCategories(Category[] categories)
        {
            if (!(categories?.Length > 1))
                return;

            try
            {
                LoggingService?.WriteSubheader("Categories");
                LoggingService?.WriteList(categories);
            }
            catch (Exception exception)
            {
                LoggingService?.WriteException(exception);
            }
        }

        #endregion

        #region Questions

        private static void PrintQuestions(Question[] questions)
        {
            if (!(questions?.Length > 1))
                return;

            try
            {
                LoggingService?.WriteSubheader("Questions");
                LoggingService?.WriteList(questions);
            }
            catch (Exception exception)
            {
                LoggingService?.WriteException(exception);
            }
        }

        #endregion

        #region Rooms

        private static void PrintRooms(Room[] rooms)
        {
            if (!(rooms?.Length > 1))
                return;

            try
            {
                LoggingService?.WriteSubheader("Rooms");
                LoggingService?.WriteList(rooms);
            }
            catch (Exception exception)
            {
                LoggingService?.WriteException(exception);
            }
        }

        #endregion

        #region Speakers

        private static void PrintSpeakers(Speaker[] speakers)
        {
            if (!(speakers?.Length > 1))
                return;

            try
            {
                LoggingService?.WriteSubheader("Speakers");
                LoggingService?.WriteList(speakers);
            }
            catch (Exception exception)
            {
                LoggingService?.WriteException(exception);
            }
        }

        #endregion

        #region Sessions

        private static void PrintSessions(Session[] sessions)
        {
            if (!(sessions?.Length > 1))
                return;

            try
            {
                LoggingService?.WriteSubheader("Sessions");
                LoggingService?.WriteList(sessions);

            }
            catch (Exception exception)
            {
                LoggingService?.WriteException(exception);
            }
        }

        #endregion
    }
}
