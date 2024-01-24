using System.Diagnostics;
using SessionizeApi.Importer.Logger;
using XamarinFiles.FancyLogger;
using static SessionizeApi.Importer.Serialization.Configurations;
using static SessionizeApi.Shared.EventIds;
using static SessionizeApi.Shared.EventTester;

namespace SessionizeApi.Importer.Tests.Smoke
{
    internal static class Program
    {
        #region Fields

        //private const string EventId = SessionizeSampleId;
        private const string EventId = SessionizeSampleId;

        #endregion

        #region Services

        private static FancyLogger? FancyLogger { get; }

        //private static AssemblyLogger? AssemblyLogger { get; }

        private static EventImporter EventImporterService { get; }

        private static EventPrinter EventPrinterService { get; }

        #endregion

        #region Constructor

        static Program()
        {
            try
            {
                // TODO Centralize?
                var writeJsonOptions = GetDebugJsonOptions(true);

                FancyLogger = new FancyLogger(writeJsonOptions: writeJsonOptions);
                //AssemblyLogger = new AssemblyLogger(FancyLogger);

                EventImporterService = new EventImporter(FancyLogger);
                EventPrinterService = new EventPrinter(FancyLogger);
            }
            catch (Exception exception)
            {
                if (FancyLogger is not null)
                {
                    FancyLogger.LogException(exception);
                }
                else
                {
                    Debug.WriteLine("ERROR: Problem setting up logging services");
                    Debug.WriteLine(exception);
                }
            }
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
