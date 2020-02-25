using FancyLogger;
using SessionizeApi.Importer;
using SessionizeApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace SessionizeApi.SocialMedia
{
    public class CsvExporter
    {
        #region Services

        private static FancyLoggerService LoggingService { get; set; }

        private static SessionizeImporter SessionizeImporter { get; set; }

        #endregion

        #region Constructor

        public CsvExporter(FancyLoggerService loggingService)
        {
            LoggingService = loggingService;
            SessionizeImporter = new SessionizeImporter(LoggingService);
        }

        #endregion

        #region Write Sessionize Event Data to CSV File

        public void WriteSpeakerThanksFromFileToCsv(string jsonFilename,
            string csvFilename = null)
        {
            try
            {
                var @event =
                    SessionizeImporter.ImportAllDataFromFile(jsonFilename,
                        // Dependency dictionaries are used to join speakers to sessions
                        includeDependencies: true);

                csvFilename = CreateCsvFilename(csvFilename, jsonFilename);

                WriteSpeakerThanksToCsv(@event, csvFilename);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }
        }

        public void WriteSpeakerThanksFromUriToCsv(Uri jsonUri, string csvFilename = null)
        {
            try
            {
                var @event =
                    SessionizeImporter.ImportAllDataFromUri(jsonUri,
                        // Dependency dictionaries are used to join speakers to sessions
                        includeDependencies: true);

                csvFilename = CreateCsvFilename(csvFilename, jsonUri);

                WriteSpeakerThanksToCsv(@event, csvFilename);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
            }

        }

        private string CreateCsvFilename(string csvFilename, string jsonFilename)
        {
            if (!string.IsNullOrWhiteSpace(csvFilename))
                return csvFilename;

            csvFilename = Path.ChangeExtension(jsonFilename, "csv");

            return csvFilename;
        }

        private string CreateCsvFilename(string csvFilename, Uri jsonUri)
        {
            if (!string.IsNullOrWhiteSpace(csvFilename))
                return csvFilename;

            // Turn it into a relative path in the bin folder
            var jsonPath = jsonUri.AbsolutePath.Trim('/');

            csvFilename = Path.ChangeExtension(jsonPath, "csv");

            return csvFilename;
        }

        private void WriteSpeakerThanksToCsv(Event @event, string csvFilename)
        {
            var thanksRecords = CreateThanksRecords(@event);
            LoggingService.WriteList<SpeakerThanks>(thanksRecords);

            // Create the relative path in the bin folder if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(csvFilename));

            using(var streamWriter = new StreamWriter(csvFilename, false))
            {
                using(var csvWriter =
                    new CsvHelper.CsvWriter(streamWriter, CultureInfo.InvariantCulture))
                {
                    csvWriter.WriteRecords(thanksRecords);
                }
            }
        }

        private IList<SpeakerThanks> CreateThanksRecords(Event @event)
        {
            var thanksRecords = new List<SpeakerThanks>();

            foreach (var session in @event.Sessions)
            {
                foreach (var speaker in session.Speakers)
                {
                    var speakerThanks = new SpeakerThanks(speaker, session);

                    // REUSE Customize this for your organization
                    var twitterMessage =
                        $"Thank you {speakerThanks.FullName} ";
                    if(!string.IsNullOrEmpty(speakerThanks.TwitterHandle))
                        twitterMessage += $"({speakerThanks.TwitterHandle}) ";
                    twitterMessage += $"for speaking at #OrlandoCC! " +
                        $"{speakerThanks.FirstName}'s session is " +
                        $"\"{speakerThanks.Title}\".";

                    speakerThanks.TwitterMessage = twitterMessage;

                    thanksRecords.Add(speakerThanks);
                }
            }

            return thanksRecords;
        }

        #endregion
    }
}
