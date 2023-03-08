using SessionizeApi.Importer.Logger;
using static SessionizeApi.Shared.EventIds;
using static SessionizeApi.Shared.EventTester;

namespace SessionizeApi.Importer.Tests.Smoke
{
    internal static class Program
    {
        #region Fields

        private const string EventId = SessionizeSampleId;

        #endregion

        #region Services

        private static LoggingService LoggingService { get; }

        private static EventImporter EventImporterService { get; }

        private static EventPrinter EventPrinterService { get; }

        #endregion

        #region Constructor

        static Program()
        {
            LoggingService = new LoggingService();
            EventImporterService = new EventImporter(LoggingService);
            EventPrinterService = new EventPrinter(LoggingService);
        }

        #endregion

        #region Testing

        internal static void Main()
        {
            LoadEventFromUrlAndPrint(EventId, EventImporterService,
                EventPrinterService);
        }

        #endregion
    }
}
