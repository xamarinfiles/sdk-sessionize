using Microsoft.Extensions.Logging;
using System;

namespace SessionizeApi.Importer.Logger
{
    internal static class LoggerCreator
    {
        #region Public

        public static ILogger CreateLogger<T>()
        {
            var loggerFactory = CreateLoggerFactory();
            var logger = loggerFactory.CreateLogger<T>();

            logger.LogInformation("Logger created" + Environment.NewLine);

            return logger;
        }

        public static ILogger CreateLogger(string categoryName)
        {
            var loggerFactory = CreateLoggerFactory();
            var logger = loggerFactory.CreateLogger(categoryName);

            logger.LogInformation("Logger created" + Environment.NewLine);

            return logger;
        }

        #endregion

        #region Private

        private static ILoggerFactory CreateLoggerFactory()
        {
            // create a logger factory
            var loggerFactory = LoggerFactory.Create(
                builder => builder
#if DEBUG
                    .AddDebug()
                    .SetMinimumLevel(LogLevel.Trace)
#else
                    .SetMinimumLevel(LogLevel.Information)
#endif
            );

            return loggerFactory;
        }

        #endregion
    }
}
