using SessionizeApi.Importer.Logger;
using SessionizeApi.Importer.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SessionizeApi.Importer
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class EventImporter
    {
        #region Fields

        // Instantiate once per application, rather than per-use. See Remarks in Doc.
        private static readonly HttpClient HttpClient = new();

        // True = avoid duplicate logging in api and client
        private const bool SkipOnSuccess = true;

        #endregion

        #region Constructor

        public EventImporter(LoggingService loggingService)
        {
            LoggingService = loggingService;
        }

        #endregion

        #region Service Properies

        private LoggingService LoggingService { get; }

        #endregion

        #region All Data (Full Event) Importer

        public Event ImportAllDataFromFile(string jsonFilename)
        {
            if (string.IsNullOrWhiteSpace(jsonFilename))
                return LogImportError(jsonFilename);

            LoggingService.LogValue("Event File", jsonFilename);

            var allDataJson = GetTextFromFile(jsonFilename);

            var @event = ImportAllDataFromJson(allDataJson,
                jsonFilename);

            return @event;
        }

        public async Task<Event> ImportAllDataFromUri(Uri jsonUri)
        {
            if (jsonUri == null)
                return LogImportError(null);

            var allDataJson = await GetTextFromUrl(jsonUri);

            var @event = ImportAllDataFromJson(allDataJson,
                jsonUri.AbsoluteUri);

            return @event;
        }

        private Event ImportAllDataFromJson(string allDataJson, string eventSource)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(allDataJson))
                    return LogImportError(eventSource);

                var @event = Event.Create(allDataJson, LoggingService, eventSource);

                LoggingService.LogObjectAsJson<Event>(@event,
                    SkipOnSuccess);

                return @event ?? LogImportError(eventSource);
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);

                return LogImportError(eventSource);
            }
        }

        private Event LogImportError(string eventSource)
        {
            LoggingService.LogError(
                $"Unable to import data from '{(eventSource ?? "NULL")}'");

            return null;
        }

        #endregion

        #region File Handling

        // TODO Convert to async when > .NET Standard 2.0 for Xamarin.Forms
        private string GetTextFromFile(string filePath)
        {
            try
            {
                var fileText = File.ReadAllText(filePath);

                return fileText;
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);

                return null;
            }
        }

        private async Task<string> GetTextFromUrl(Uri fileUri)
        {
            try
            {
                var responseBody = await HttpClient.GetStringAsync(fileUri);

                return responseBody;
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);

                return null;
            }

        }

        #endregion
    }
}
