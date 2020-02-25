using FancyLogger;
using SessionizeApi.Importer;
using SessionizeApi.Importer.Events;
using SessionizeApi.Samples;
using SessionizeApi.SocialMedia;
using System;

namespace SessionizeApi.SocialMedia.Console
{
    internal static class Program
    {
        #region Services

        private static FancyLoggerService LoggingService { get; set; }

        private static SessionizeImporter SessionizeImporter { get; set; }

        private static CsvExporter CsvExporter { get; set; }

        #endregion

        #region Testing

        internal static void Main()
        {
            try
            {
                LoggingService = new FancyLoggerService();
                SessionizeImporter = new SessionizeImporter(LoggingService);
                CsvExporter = new CsvExporter(LoggingService);

                var occ2020files = new Filenames.Event.Local("OrlandoCodeCamp", 2020);
                ExportSpeakerThanksFromFile(occ2020files.AllData);

                // REUSE Pass your Sessionize ID (below is their sample event ID)
                var occ2020web = new Filenames.Event.Web("jl4ktls0");
                ExportSpeakerThanksFromUri(occ2020web.AllData);

            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        private static void ExportSpeakerThanksFromFile(string jsonFilename)
        {
            try
            {
                var @event =
                    SessionizeImporter.ImportAllDataFromFile(jsonFilename,
                        CustomSort.CollectSpeakerSessionsByFirstSubmission);

                CsvExporter.WriteSpeakerThanksFromFileToCsv(jsonFilename);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        private static void ExportSpeakerThanksFromUri(Uri jsonUri)
        {
            try
            {
                var @event =
                    SessionizeImporter.ImportAllDataFromUri(jsonUri,
                        CustomSort.CollectSpeakerSessionsByFirstSubmission);

                CsvExporter.WriteSpeakerThanksFromUriToCsv(jsonUri);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        #endregion
    }
}
