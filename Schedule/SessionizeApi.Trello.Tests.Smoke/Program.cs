using SessionizeApi.Importer;
using SessionizeApi.Importer.Logger;
using SessionizeApi.Importer.Models;
using System.Diagnostics.CodeAnalysis;
using static SessionizeApi.Importer.SessionizeImporter;
#pragma warning disable IDE0051

namespace SessionizeApi.Trello.Tests.Smoke
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
    internal static class Program
    {
        // TODO Centralize these
        #region Sessionize Fields

        private const string SessionizeSampleId = "** TODO **";

        private const string Occ2023Id = "** TODO **";

        #endregion

        #region Trello Fields

        private const string TrelloApiKey = "** TODO **";

        private const string TrelloApiToken = "** TODO **";

        private const string SessionizeBoardId = "** TODO **"; // Sessionize Sample

        private const string TrelloBoardId = SessionizeBoardId;

        #endregion

        #region Services

        //private static CsvExporter CsvExporter { get; set; }

        private static LoggingService LoggingService { get; } = new();

        private static SessionizeImporter SessionizeImporter { get; } =
            new(LoggingService);

        private static SessionizeLogger SessionizeLogger { get; } = new(LoggingService);

        private static TrelloExporter TrelloExporter { get; set; } =
            new(LoggingService, TrelloApiKey, TrelloApiToken);

        #endregion

        #region Setup

        internal static async Task Main()
        {
            try
            {
                var @event = LoadAndPrintEventFromUrl(SessionizeSampleId);

                // var @event = LoadAndPrintEventFromUrl(Occ2023Id);

                if (@event is null)
                    return;

                await ExportEventSessions(@event, TrelloBoardId);
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);
            }
        }

        #endregion

        #region Private - Export to Trelo

        private static async Task ExportEventSessions(Event @event, string boardId)
        {
            try
            {
                await TrelloExporter.ConnectToTrelloAndLoadBoard(@event, boardId);
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);
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
                Task.Run(() => SessionizeImporter.ImportAllDataFromUri(webUrl))
                    .GetAwaiter().GetResult();

            SessionizeLogger.PrintEvent(webEvent);

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
