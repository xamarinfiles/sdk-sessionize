using SessionizeApi.Importer;
using SessionizeApi.Importer.Logger;
using SessionizeApi.Importer.Models;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using XamarinFiles.FancyLogger;
//#pragma warning disable IDE0051
using static SessionizeApi.Importer.Serialization.Configurations;
using static SessionizeApi.Shared.EventIds;
using static SessionizeApi.Shared.EventTester;

namespace SessionizeApi.Trello.Tests.Smoke
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
    internal static class Program
    {
        // TODO Move to config files
        #region Sessionize Fields

        //private const string EventId = SessionizeSampleId;
        //private const string EventId = OrlandoCodeCamp2023Id;
        private const string EventId = OrlandoCodeCamp2024Id;

        #endregion

        #region Trello Fields

        private const string TrelloApiKey = "** TODO **";

        private const string TrelloApiToken = "** TODO **";

        private const string SessionizeBoardId = "** TODO **"; // Sessionize Sample

        private const string TrelloBoardId = SessionizeBoardId;

        #endregion

        #region Services

        private static EventImporter? EventImporter { get; }

        private static EventPrinter? EventPrinter { get; }

        private static FancyLogger? FancyLogger { get; }

        private static TrelloExporter? TrelloExporter { get; }

        #endregion

        #region User Secrets

        // TODO
        //private static IConfigurationRoot Configuration;
        //private const string _trelloApiKeySecretName = "TrelloApiKey";
        //private const string _trelloApiTokenSecretName = "TrelloApiToken";
        //private const string _trelloBoardIdSecretName = "TrelloBoardId";

        #endregion

        #region Constructor

        static Program()
        {
            try
            {
                // TODO Centralize?
                var writeJsonOptions = GetDebugJsonOptions(true);

                FancyLogger = new FancyLogger(writeJsonOptions: writeJsonOptions);
                EventImporter = new EventImporter(FancyLogger);
                EventPrinter = new EventPrinter(FancyLogger);
                TrelloExporter = new TrelloExporter(FancyLogger,
                    // TODO
                    TrelloApiKey, TrelloApiToken);
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

        #region Setup

        internal static async Task Main()
        {
            try
            {
                var @event = LoadAndPrintEventFromUrl(EventId);

                if (@event is null)
                    return;

                await ExportEventSessions(@event, TrelloBoardId);
            }
            catch (Exception exception)
            {
                FancyLogger?.LogException(exception);
            }
        }

        #endregion

        #region Private - Export to Trelo

        private static async Task ExportEventSessions(Event @event, string boardId)
        {
            try
            {
                if (TrelloExporter is null)
                    return;

                await TrelloExporter.ConnectToTrelloAndLoadBoard(@event, boardId);
            }
            catch (Exception exception)
            {
                FancyLogger?.LogException(exception);
            }
        }

        #endregion

        // TODO Consolidate with SessionizeApi.Importer.Tests.Smoke
        #region Private - Import from Sessionize

        // TODO Add file equivalent

        private static Event? LoadAndPrintEventFromUrl(string eventId)
        {
            var webUrl = PopulateUrl(eventId);
            var webEvent =
                Task.Run(() => EventImporter?.ImportAllDataFromUri(webUrl))
                    .GetAwaiter().GetResult();

            EventPrinter?.PrintEvent(webEvent);

            return webEvent;
        }

        private static Uri PopulateUrl(string sessionizeId)
        {
            var sessionizeUrlStr = string.Format(SessionizeUrlFormat, sessionizeId);
            var sessionizeUrl = new Uri(sessionizeUrlStr);

            return sessionizeUrl;
        }

        #endregion
    }
}
