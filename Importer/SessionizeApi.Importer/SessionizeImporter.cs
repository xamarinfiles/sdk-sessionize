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
    public class SessionizeImporter
    {
        #region Fields

        public const string SessionizeUrlFormat =
            "https://sessionize.com/api/v2/{0}/view/All";

        // Instantiate once per application, rather than per-use. See Remarks in Doc.
        private static readonly HttpClient HttpClient = new();

        // True = avoid duplicate logging in api and client
        private const bool SkipOnSuccess = true;

        #endregion

        #region Constructor

        public SessionizeImporter(LoggingService loggingService)
        {
            LoggingService = loggingService;
        }

        #endregion

        #region Services

        private LoggingService LoggingService { get; }

        #endregion

        #region All Data (Full Event) Importer

        public Event ImportAllDataFromFile(string jsonFilename)
        {
            if (string.IsNullOrWhiteSpace(jsonFilename))
                return LogImportError(jsonFilename);

            //LoggingService.LogValue("Event File", jsonFilename);

            try
            {
                var allDataJson = GetTextFromFile(jsonFilename);

                var @event = ImportAllDataFromJson(allDataJson,
                    jsonFilename);

                return @event;
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);

                return LogImportError(jsonFilename);
            }
        }

        public async Task<Event> ImportAllDataFromUri(Uri jsonUri)
        {
            if (jsonUri == null)
                return LogImportError(null);

            //LoggingService.LogValue("Event URL", jsonUri.ToString());

            try
            {
                var allDataJson = await GetTextFromUrl(jsonUri);

                var @event = ImportAllDataFromJson(allDataJson,
                    jsonUri.AbsoluteUri);

                return @event;
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);

                return LogImportError(jsonUri.OriginalString);
            }
        }

        private Event ImportAllDataFromJson(string allDataJson,
            string eventSource = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(allDataJson))
                    return LogImportError(eventSource);

                var allData =
                    AllData.FromJson(allDataJson, LoggingService, eventSource);

                if (allData == null)
                    return LogImportError(eventSource);

                LoggingService.LogObjectAsJson<AllData>(allData,
                    SkipOnSuccess);

                var @event = Event.Create(allData);

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
