using FancyLogger;
using SessionizeApi.Loader;
using SessionizeApi.Logger;
using SessionizeApi.Samples;
using System;
using System.Diagnostics;

namespace SessionizeApi.Models.Console
{
    internal static class Program
    {
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

                ImportAndPrintEvent(Filenames.OrlandoCodeCamp2020AllData);
            }
            catch (Exception exception)
            {
                LoggingService?.WriteException(exception);
            }
        }

        #endregion

        #region Debugging / Discovery / Verification

        [Conditional("DEBUG")]
        private static void ImportAndPrintEvent(string eventJsonFileName)
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