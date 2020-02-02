using FancyLogger;
using SessionizeApi.Importer;
using SessionizeApi.Importer.Events;
using SessionizeApi.Logger;
using SessionizeApi.Samples;
using System;

namespace SessionizeApi.Models.Console
{
    internal static class Program
    {
        #region Services

        private static FancyLoggerService LoggingService { get; set; }

        private static SessionizeImporter SessionizeImporter { get; set; }

        private static SessionizeLogger SessionizeLogger { get; set; }

        #endregion

        #region Setup

        internal static void Main()
        {
            try
            {
                LoggingService = new FancyLoggerService();
                SessionizeLogger = new SessionizeLogger(LoggingService);
                SessionizeImporter = new OrlandoCodeCampSessionizeImporter(LoggingService);

                ImportAndPrintEvent(Filenames.OrlandoCodeCamp2020AllData);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        #endregion

        #region Debugging / Discovery / Verification

        private static void ImportAndPrintEvent(string eventJsonFileName)
        {
            try
            {
                var @event =
                    SessionizeImporter.LoadEvent(eventJsonFileName,
                        CustomSort.CollectSpeakerSessionsByFirstSubmission);

                SessionizeLogger.PrintEvent(@event);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        #endregion
    }
}