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
        public void PrintEvent(Event @event, bool? printDetails = false)
        {
            if (@event == null)
                return;

            try
            {
                var header = string.IsNullOrEmpty(@event.DebuggerDisplay)
                    ? "All Data"
                    : @event.DebuggerDisplay;

                FancyLogger.LogHeader(header);
                PrintEventDetails(@event, printDetails);
                PrintEventSummary(@event);
            }
            catch (Exception exception)
            {
                FancyLogger.LogException(exception);
            }
        }

        [Conditional("DEBUG")]
        private void PrintEventDetails(Event @event, bool? printDetails = false)
        {
            if (printDetails == false)
                return;

            PrintArrayContents(@event.Speakers, "Speakers");
            PrintArrayContents(@event.Sessions, "Sessions");
            PrintArrayContents(@event.Questions, "Questions");
            PrintArrayContents(@event.Choices, "Choices");
            PrintArrayContents(@event.Rooms, "Rooms");
        }

        [Conditional("DEBUG")]
        private void PrintArrayContents<T>(IReadOnlyList<T> array, string label)
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

                    FancyLogger.LogInfo("{0}: {1}", addIndent: false,
                        newLineAfter:true, index + 1, obj.LogDisplayLong);

                    FancyLogger.LogObject<T>(obj);
                }
            }
            catch (Exception exception)
            {
                FancyLogger.LogException(exception);
            }
        }

        [Conditional("DEBUG")]
        private void PrintEventSummary(Event @event)
        {
            FancyLogger.LogHeader($"Event Summary");

            PrintArraySummary(@event.Speakers, "Speakers");
            PrintArraySummary(@event.Sessions, "Sessions");
            PrintArraySummary(@event.Questions, "Questions");
            PrintArraySummary(@event.Choices, "Choices");
            PrintArraySummary(@event.Rooms, "Rooms", newLineAfter: true);
        }

        [Conditional("DEBUG")]
        private void PrintArraySummary<T>(IReadOnlyCollection<T> array,
            string label, bool newLineAfter = false)
        {
            FancyLogger.LogInfo("{0,3} {1}", addIndent: true,
                newLineAfter: newLineAfter,
                array?.Count.ToString() ?? "0", label );
        }

        #endregion
    }
}
