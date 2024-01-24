using SessionizeApi.Importer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using XamarinFiles.FancyLogger;

namespace SessionizeApi.Importer.Logger
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class EventPrinter
    {
        #region Constructor

        public EventPrinter(IFancyLogger fancyLogger)
        {
            FancyLogger = fancyLogger;
        }

        #endregion

        #region Services

        private IFancyLogger FancyLogger { get; }

        #endregion

        #region Debugging / Discovery / Verification

        [Conditional("DEBUG")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public void PrintEvent(Event @event, bool printEvent)
        {
            if (@event == null || printEvent == false)
                return;

            try
            {
                var header = string.IsNullOrEmpty(@event.DebuggerDisplay)
                    ? "All Data"
                    : @event.DebuggerDisplay;

                FancyLogger.LogHeader(header);

                PrintArray(@event.Speakers, "Speakers");
                PrintArray(@event.Sessions, "Sessions");
                PrintArray(@event.Questions, "Questions");
                PrintArray(@event.Categories, "Categories");
                PrintArray(@event.Rooms, "Rooms");
            }
            catch (Exception exception)
            {
                FancyLogger.LogException(exception);
            }
        }

        [Conditional("DEBUG")]
        private void PrintArray<T>(IReadOnlyList<T> array, string label)
            where T : ILogFormattable
        {
            // Make sure array is not null or empty
            if (!(array?.Count > 1))
                return;

            try
            {
                FancyLogger.LogHeader($"{array.Count} {label}");

                for (var index = 0; index < array.Count; index++)
                {
                    ILogFormattable obj = array[index];

                    FancyLogger.LogInfo("{0}: {1}", addIndent: true,
                        newLineAfter:true, index + 1, obj.LogDisplayLong);

                    FancyLogger.LogObject<T>(obj);
                }
            }
            catch (Exception exception)
            {
                FancyLogger.LogException(exception);
            }
        }

        #endregion
    }
}
