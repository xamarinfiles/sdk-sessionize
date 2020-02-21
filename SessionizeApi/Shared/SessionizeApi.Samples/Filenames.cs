using System;

namespace SessionizeApi.Samples
{
    internal struct Filenames
    {
        private const string AllDataUrlFormat =
            "https://sessionize.com/api/v2/{0}/view/All";

        private const string SmartGridUrlFormat =
            "https://sessionize.com/api/v2/{0}/view/GridSmart";

        private const string SessionsUrlFormat =
            "https://sessionize.com/api/v2/{0}/view/Sessions";

        private const string SpeakersUrlFormat =
            "https://sessionize.com/api/v2/{0}/view/Speakers";

        private const string SpeakerWallUrlFormat =
            "https://sessionize.com/api/v2/{0}/view/SpeakerWall";

        #region API Samples

        internal struct Sessionize
        {
            internal struct Local
            {
                internal const string AllData =
                    "Sessionize\\All.json";

                internal const string SmartGrid =
                    "Sessionize\\GridSmart.json";

                internal const string Sessions =
                    "Sessionize\\Sessions.json";

                internal const string Speakers =
                    "Sessionize\\Speakers.json";

                internal const string SpeakerWall =
                    "Sessionize\\SpeakerWall.json";
            }

            internal struct Web
            {
                internal const string _sampleEvent = "jl4ktls0";

                internal static readonly string AllData =
                    string.Format(AllDataUrlFormat, _sampleEvent);

                internal static readonly string Schedule =
                    string.Format(SmartGridUrlFormat, _sampleEvent);

                internal static readonly string Sessions =
                    string.Format(SessionsUrlFormat, _sampleEvent);

                internal static readonly string Speakers =
                    string.Format(SpeakersUrlFormat, _sampleEvent);

                internal static readonly string SpeakerWall =
                    string.Format(SpeakerWallUrlFormat, _sampleEvent);
            }
        }

        #endregion

        #region Events

        internal struct Event
        {
            internal struct Local
            {
                public Local(string eventFolder, int year)
                {
                    EventFolder = eventFolder;
                    Year = year;
                }

                internal string EventFolder { get; private set; }

                internal int Year { get; private set; }

                internal string AllData =>
                    $"Events\\{EventFolder}\\{Year}\\All.json";
            }

            internal struct Web
            {
                public Web(string eventId) => EventId = eventId;

                internal string EventId { get; private set; }

                internal Uri AllData => new Uri(string.Format(AllDataUrlFormat, EventId));
            }
        }

        #endregion
    }
}
