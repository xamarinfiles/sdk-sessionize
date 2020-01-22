using FancyLogger;
using SessionizeApi.Loader;
using SessionizeApi.Logger;
using System;

namespace SessionizeApi.Models.Console
{
    internal static class Program
    {
        #region Sample Files

        private static readonly string AllDataJsonFile = "Events\\OrlandoCodeCamp\\2020-All.json";

        //private static readonly string AllDataJsonFile = "All.json";

        //private static readonly string GridSmartJsonFile = "GridSmart.json";

        //private static readonly string SessionsJsonFile = "Sessions.json";

        //private static readonly string SpeakersJsonFile = "Speakers.json";

        //private static readonly string SpeakerWallJsonFile = "SpeakerWall.json";

        #endregion

        #region Services

        private static FancyLoggerService LoggingService { get; set; }

        #endregion

        #region Main

        public static void Main()
        {
            try
            {
                LoggingService = new FancyLoggerService();

                SessionizeLoader.LoggingService = LoggingService;
                SessionizeLogger.LoggingService = LoggingService;

                TestEvent(AllDataJsonFile);
            }
            catch (Exception exception)
            {
                LoggingService?.WriteException(exception);
            }
        }

        #endregion

        #region All Data

        private static void TestEvent(string eventJsonFileName)
        {
            try
            {
                var @event = SessionizeLoader.LoadEvent(eventJsonFileName);

                SessionizeLogger.PrintEvent(@event);
            }
            catch (Exception exception)
            {
                LoggingService?.WriteException(exception);
            }
        }

        #endregion
    }
}