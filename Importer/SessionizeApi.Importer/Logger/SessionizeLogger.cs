using SessionizeApi.Importer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace SessionizeApi.Importer.Logger
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class SessionizeLogger
    {
        #region Services

        private static LoggingService LoggingService { get; set; }

        #endregion

        #region Constructor

        public SessionizeLogger(LoggingService loggingService)
        {
            LoggingService = loggingService;
        }

        #endregion

        #region Debugging / Discovery / Verification

        [Conditional("DEBUG")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static void PrintEvent(Event @event)
        {
            if (@event == null)
                return;

            try
            {
                var header = string.IsNullOrEmpty(@event.DebuggerDisplay)
                    ? "All Data"
                    : @event.DebuggerDisplay;

                LoggingService.LogHeader(header);

                PrintArray(@event.Speakers, "Speakers");
                PrintArray(@event.Sessions, "Sessions");
                PrintArray(@event.Questions, "Questions");
                PrintArray(@event.Categories, "Categories");
                PrintArray(@event.Rooms, "Rooms");
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);
            }
        }

        [Conditional("DEBUG")]
        private static void PrintArray<T>(IReadOnlyList<T> array, string label)
            where T : ILogFormattable
        {
            if (!(array?.Count > 1))
                return;

            try
            {
                LoggingService.LogHeader($"{array.Count} {label}");

                for (var index = 0; index < array.Count; index++)
                {
                    ILogFormattable obj = array[index];

                    LoggingService.LogSubheader("{0}: {1}",
                        index + 1, obj.LogDisplayLong);

                    LoggingService.LogObjectAsJson<T>(obj);
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);
            }
        }

        #endregion
    }
}
