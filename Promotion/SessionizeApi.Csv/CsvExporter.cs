using CsvHelper;
using SessionizeApi.Importer.Logger;
using SessionizeApi.Importer.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace SessionizeApi.Csv
{
    public class CsvExporter
    {
        #region URL Fields

        private const string BaseSiteUrl = "https://orlandocodecamp.com/";

        private const string MastodonUriFormat = "https://{0}/@{1}";

        private const string MastodonHandleFormat = "@{0}@{1}";

        private const string RelativeSessionUrl =
            BaseSiteUrl + "/session/#sz-session-";

        #endregion

        // TODO Replace with enum
        #region Social Media Fields

        private const string LinkedIn = "LinkedIn";

        private const string Mastodon = "Mastodon";

        private const string Twitter = "Twitter";

        #endregion

        #region Constructor

        public CsvExporter(LoggingService loggingService)
        {
            LoggingService = loggingService;
        }

        #endregion

        #region Service Properies

        private static LoggingService LoggingService { get; set; }

        #endregion

        #region CSV Methods

        // TODO Make it work with general incoming filename and/or prefix
        // TODO Pass in speaker selection criteria
        public string ExportSpeakersToCsv(Event @event, string baseFilename)
        {
            try
            {
                if (@event is null || string.IsNullOrWhiteSpace(baseFilename))
                    return null;

                var csvFilename =
                    Path.ChangeExtension(baseFilename, "csv");

                var speakerData = PullSpeakerData(@event);

                LoggingService.LogHeader("CSV Export");
                LoggingService.LogObjectAsJson<List<PartialSpeaker>>(speakerData);

                using var streamWriter = new StreamWriter(csvFilename, false);
                using var csvWriter =
                    new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

                csvWriter.WriteRecords(speakerData);

                return csvFilename;
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);

                return null;
            }
        }

        private static IList<PartialSpeaker> PullSpeakerData(Event @event)
        {
            if (@event is null)
                return null;

            var mastodonQuestionId =
                @event.Questions
                    .Where(question =>
                        question.QuestionText == Mastodon)
                    .Select(question => question.Id.Number)
                    .FirstOrDefault();
            var partialSpeakers = new List<PartialSpeaker>();

            foreach (var speaker in @event.Speakers)
            {
                var partialSpeaker = new PartialSpeaker
                {
                    FullName = speaker.FullName,
                    ProfilePicture = speaker.ProfilePicture,
                };

                foreach (var link in speaker.Links)
                {
                    // TODO Update as needed
                    switch (link.Title)
                    {
                        case LinkedIn:
                            partialSpeaker.LinkedInLink = link.Url;

                            break;
                        case Mastodon:
                            var (mastodonLink, mastodonHandle) =
                                PullOutUrlAndHandle(link.Url, Mastodon);

                            partialSpeaker.MastodonLink = mastodonLink;
                            partialSpeaker.MastodonHandle = mastodonHandle;

                            break;
                        case Twitter:
                            var (twitterLink, twitterHandle) =
                                PullOutUrlAndHandle(link.Url, Twitter);

                            partialSpeaker.TwitterLink = twitterLink;
                            partialSpeaker.TwitterHandle = twitterHandle;

                            break;
                    }
                }

                if (partialSpeaker.MastodonLink == null
                    || string.IsNullOrWhiteSpace(partialSpeaker.MastodonHandle))
                {
                    var mastodonAnswerText =
                        speaker.QuestionAnswers
                            .Where(answer =>
                                answer.Id.Number == mastodonQuestionId)
                            .Select(answer => answer.AnswerText)
                            .FirstOrDefault();

                    var (mastodonLink, mastodonHandle) =
                        PullOutUrlAndHandle(mastodonAnswerText, Mastodon);

                    partialSpeaker.MastodonLink = mastodonLink;
                    partialSpeaker.MastodonHandle = mastodonHandle;
                }

                // Add one line for each session of speaker
                foreach (var sessionReference in speaker.SessionReferences)
                {

                    partialSpeaker.SessionTitle = sessionReference.Name;
                    partialSpeaker.SessionLink =
                        new Uri(RelativeSessionUrl + sessionReference.Id);

                    partialSpeakers.Add(partialSpeaker);
                }
            }

            return partialSpeakers;
        }

        #endregion

        #region URL Methods

        private static (Uri, string) PullOutUrlAndHandle(string urlStr,
            string linkSource)
        {
            if (string.IsNullOrWhiteSpace(urlStr))
                return (null, null);

            if (Uri.TryCreate(urlStr, UriKind.Absolute, out var absoluteUri)
                && IsHttpOrHttps(absoluteUri))
            {
                return ProcessAbsoluteUri(absoluteUri, linkSource);
            }

            if (Uri.TryCreate(urlStr, UriKind.Relative, out var relativeUri))
            {
                return ProcessRelativeUri(relativeUri, linkSource);
            }

            return (null, null);
        }

        private static (Uri, string) PullOutUrlAndHandle(Uri uri,
            string linkSource)
        {
            if (uri is null)
                return (null, null);

            if (uri.IsAbsoluteUri && IsHttpOrHttps(uri))
            {
                return ProcessAbsoluteUri(uri, linkSource);
            }

            return ProcessRelativeUri(uri, linkSource);
        }

        private static (Uri, string) ProcessAbsoluteUri(Uri inUri,
            string linkSource)
        {
            var outUri = inUri;
            var accountHandle = "";

            try
            {
                var segments = inUri.Segments;

                switch (linkSource)
                {
                    case Twitter when segments.Length >= 2:
                        var accountName = segments[1];

                        accountHandle =
                            accountName.StartsWith("@")
                            ? accountName
                            : "@" + accountName;

                        break;
                    case Mastodon when segments.Length == 2:
                    {
                        return RecreateMastodonValues(inUri.Host,
                            segments[1].Substring(1));
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogException(exception);
            }

            return (outUri, accountHandle);
        }

        private static (Uri, string) ProcessRelativeUri(Uri inUri,
            string linkSource)
        {
            var outUri = inUri;
            var handle = "";

            try
            {
                switch (linkSource)
                {
                    case Twitter:
                    {
                        var segments = inUri.Segments;

                        if (segments.Length == 2)
                            handle = segments[1];

                        break;
                    }
                    case Mastodon:
                    {
                        var segments =
                            inUri.OriginalString.Split('@');

                        if (segments.Length == 3)
                        {
                            return RecreateMastodonValues(segments[2],
                                segments[1]);
                        }

                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogException(exception);
            }

            return (outUri, handle);
        }

        // TODO Why doesn't this exist in .NET already? Or does it (outside of ASP.NET)? If so, where?
        // TODO Convert into Uri extension method
        private static bool IsHttpOrHttps(Uri uri)
        {
            return uri?.Scheme == Uri.UriSchemeHttp || uri?.Scheme == Uri.UriSchemeHttps;
        }

        private static (Uri, string) RecreateMastodonValues(string hostName,
            string accountName)
        {
            var uri =
                new Uri(string.Format(MastodonUriFormat, hostName, accountName));
            var handle =
                string.Format(MastodonHandleFormat, accountName, hostName);

            return (uri, handle);
        }

        #endregion
    }
}
