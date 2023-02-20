using SessionizeApi.Importer.Logger;
using SessionizeApi.Importer.Models;
using System.Diagnostics.CodeAnalysis;
using static SessionizeApi.Importer.SessionizeImporter;
#pragma warning disable IDE0051

namespace SessionizeApi.Importer.Tests.Smoke
{
    [SuppressMessage("ReSharper", "UnusedMember.Local")]
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Local")]
    internal static class Program
    {
        // TODO Centralize these
        #region Fields

        private const string SessionizeSampleId = "jl4ktls0";

        private const string Occ2023TrelloId = "f98nlbrs";

        private const string Occ2023AppId = "18e6ggtu";

        #endregion

        #region Services

        private static LoggingService LoggingService { get; } = new();

        private static SessionizeImporter SessionizeImporter { get; } =
            new(LoggingService);

        private static SessionizeLogger SessionizeLogger { get; } =
            new(LoggingService);

        #endregion

        #region Testing

        internal static void Main()
        {
            LoadAndPrintEventFromUrl(SessionizeSampleId);

            //LoadAndPrintEventFromUrl(Occ2023TrelloId);

            //LoadAndPrintEventFromUrl(Occ2023AppId);
        }

        #endregion

        #region Private

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
