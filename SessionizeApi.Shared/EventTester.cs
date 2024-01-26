using SessionizeApi.Importer;
using SessionizeApi.Importer.Logger;
using SessionizeApi.Importer.Models;
using System;
using System.Threading.Tasks;

namespace SessionizeApi.Shared
{
    public static class EventTester
    {
        #region Fields

        private const string SessionizeUrlFormat =
            "https://sessionize.com/api/v2/{0}/view/All";

        #endregion

        #region Methods

        public static Event? LoadEventFromFileAndPrint(string filename,
            EventImporter importerService, EventPrinter printerService,
            bool printEvent = true)
        {
            var fileEvent =
                Task.Run(() =>
                        importerService.ImportAllDataFromFile(filename))
                    .GetAwaiter().GetResult();

            printerService.PrintEvent(fileEvent, printEvent);

            return fileEvent;
        }

        public static Event? LoadEventFromUrlAndPrint(string eventId,
            EventImporter importerService, EventPrinter printerService,
            bool printEvent = true)
        {
            var webUrl = PopulateUrl(eventId);
            var webEvent =
                Task.Run(() =>
                        importerService.ImportAllDataFromUri(webUrl))
                    .GetAwaiter().GetResult();

            printerService.PrintEvent(webEvent, printEvent);

            return webEvent;
        }

        public static Uri PopulateUrl(string eventId,
            string urlFormat = SessionizeUrlFormat)
        {
            var sessionizeUrlStr = string.Format(urlFormat, eventId);
            var sessionizeUrl = new Uri(sessionizeUrlStr);

            return sessionizeUrl;
        }

        #endregion
    }
}
