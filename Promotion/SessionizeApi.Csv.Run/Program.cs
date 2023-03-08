using SessionizeApi.Importer;
using SessionizeApi.Importer.Logger;
using static SessionizeApi.Shared.EventIds;
using static SessionizeApi.Shared.EventTester;

namespace SessionizeApi.Csv.Run
{
    internal static class Program
    {
        #region Fields

        private const string EventId = SessionizeSampleId;

        #endregion

        #region Services

        private static CsvExporter CsvExporterService { get; }

        private static EventImporter EventImporterService { get; }

        private static EventPrinter EventPrinterService { get; }

        private static LoggingService LoggingService { get; }

        #endregion

        #region Constructor

        static Program()
        {
            LoggingService = new LoggingService();
            EventImporterService = new EventImporter(LoggingService);
            EventPrinterService = new EventPrinter(LoggingService);
            CsvExporterService = new CsvExporter(LoggingService);
        }

        #endregion

        #region Testing

        static void Main(string[] args)
        {
            LoadEventFromUrlAndExportAsCsv(EventId);
        }

        #endregion

        #region Private

        // TODO Add file equivalent

        private static string? LoadEventFromUrlAndExportAsCsv(string eventId)
        {
            var webEvent = LoadEventFromUrlAndPrint(eventId, EventImporterService,
                EventPrinterService);

            if (webEvent == null)
            {
                return null;
            }

            var csvFilename =
                CsvExporterService.ExportSpeakersToCsv(webEvent, eventId);

            return csvFilename;
        }

        #endregion
    }
}
