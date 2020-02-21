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

                var occ2020files = new Filenames.Event.Local("OrlandoCodeCamp", 2020);
                ImportAndPrintEventFromFile(occ2020files.AllData);

                // TODO Pass your Sessionize ID (below is their sample event ID)
                var occ2020web = new Filenames.Event.Web("jl4ktls0");
                ImportAndPrintEventFromUri(occ2020web.AllData);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        #endregion

        #region Debugging / Discovery / Verification

        private static void ImportAndPrintEventFromFile(string eventJsonFileName)
        {
            try
            {
                var @event =
                    SessionizeImporter.ImportAllDataFromFile(eventJsonFileName,
                        CustomSort.CollectSpeakerSessionsByFirstSubmission);

                SessionizeLogger.PrintEvent(@event);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        private static void ImportAndPrintEventFromUri(Uri eventJsonUri)
        {
            try
            {
                var @event =
                    SessionizeImporter.ImportAllDataFromUri(eventJsonUri,
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