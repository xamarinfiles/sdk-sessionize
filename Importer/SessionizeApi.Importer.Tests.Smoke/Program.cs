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
        // TODO Move to config files
        #region Sessionize Fields

        //private const string EventId = SessionizeSampleId;
        //private const string EventId = OrlandoCodeCamp2023Id;
        private const string EventId = OrlandoCodeCamp2024Id;

        #endregion

        #region Services

        private static FancyLogger? FancyLogger { get; }

        //private static AssemblyLogger? AssemblyLogger { get; }

        private static EventImporter EventImporter { get; }

        private static EventPrinter EventPrinter { get; }

        #endregion

        #region Constructor

        static Program()
        {
            try
            {
                FancyLogger = new FancyLogger(
                    writeJsonOptions: DefaultWriteJsonOptionsWithNull);
                //AssemblyLogger = new AssemblyLogger(FancyLogger);
                EventImporter = new EventImporter(FancyLogger);
                EventPrinter = new EventPrinter(FancyLogger);
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
            LoadEventFromUrlAndPrint(EventId, EventImporter, EventPrinter);
        }

        #endregion
    }
}
