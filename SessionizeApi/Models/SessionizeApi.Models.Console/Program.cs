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

        private static SessionizeLoader SessionizeLoader { get; set; }

        private static SessionizeLogger SessionizeLogger { get; set; }

        #endregion

        #region Setup

        internal static void Main()
        {
            try
            {
                LoggingService = new FancyLoggerService();

                SessionizeLogger = new SessionizeLogger(LoggingService);

                SessionizeLoader = new SessionizeLoader(LoggingService);

                ImportAndPrintEvent(Filenames.OrlandoCodeCamp2020AllData);
            }
            catch (Exception exception)
            {
                LoggingService.WriteException(exception);
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
                LoggingService.WriteException(exception);
            }
        }

        #endregion
    }
}