using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.Json;
using static SessionizeApi.Importer.Constants.Characters;
using static SessionizeApi.Importer.Serialization.Formatting;
using static System.Net.HttpStatusCode;
using static System.Net.Sockets.SocketError;
using static System.Net.WebExceptionStatus;

namespace SessionizeApi.Importer.Logger
{
    // TODO Replace with Fancy Logger NuGet
    // TODO Turn into proper service for DI
    // TODO Drop suppression and update switch when > .NET Standard 2.0 for Xamarin.Forms
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class LoggingService
    {
        #region Fields

        private readonly ILogger _logger;

        // TODO Pass these into constructor
        private const int HeaderLength = 70;
        private const char HeaderPaddingChar = '#';

        #endregion

        #region Constructors

        // TODO Test with other log destinations after finish merging code
        public LoggingService(ILogger logger = null,
            string loggerName = "LOG")
        {
            _logger = logger ?? LoggerCreator.CreateLogger(loggerName);
        }

        #endregion

        #region Public - Error Logging

        public void LogError(string format, params object[] args)
        {
            var message = string.Format(format, args).Trim() + ' ';

            _logger.LogError("ERROR:  {message}", message);
        }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public void LogException(Exception exception)
        {
            LogCommonException(exception, "EXCEPTION");
        }


        public void LogExceptionRouter(Exception exception)
        {
            switch (exception)
            {
                // TODO
                //case ValidationApiException validationException:
                //    LogApiException(validationException);

                //    break;
                // TODO
                //case ApiException apiException:
                //    LogApiException(apiException);

                //    break;
                case HttpRequestException httpRequestException:
                    LogHttpRequestException(httpRequestException);

                    break;
                case JsonException jsonException:
                    LogJsonException(jsonException);

                    break;
                default:
                    LogException(exception);

                    break;
            }
        }

        [SuppressMessage("ReSharper", "MergeIntoPattern")]
        public void LogHttpRequestException(HttpRequestException requestException)
        {
            // TODO Pull this when update to .NET 7 and add to label
            // HttpStatusCode? outerStatusCode = requestException.StatusCode;
            const string outerExceptionLabel = "HTTP REQUEST EXCEPTION";

            string innerExceptionLabel;
            HttpStatusCode? innerStatusCode = null;

            // TODO Add more networking error conditions
            switch (requestException.InnerException)
            {
                case SocketException socketException
                    when socketException.SocketErrorCode == ConnectionRefused:

                    innerExceptionLabel =
                        "SOCKET EXCEPTION - ConnectionRefused";

                    innerStatusCode = ServiceUnavailable;

                    break;
                case WebException webException
                    when webException.Status == NameResolutionFailure:

                    innerExceptionLabel =
                        "WEB EXCEPTION - NameResolutionFailure";

                    innerStatusCode = ServiceUnavailable;

                    break;

                default:

                    innerExceptionLabel = "INNER EXCEPTION";

                    break;
            }

            innerExceptionLabel += $"{innerStatusCode?.ToString() ?? ""}";

            LogCommonException(requestException, outerExceptionLabel,
                innerExceptionLabel);
        }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public void LogJsonException(JsonException jsonException)
        {
            LogCommonException(jsonException, "JSON EXCEPTION");
        }

        #endregion

        #region Public - Detail Logging [DEBUG only]

        [Conditional("DEBUG")]
        public void LogBlankLine()
        {
            _logger.LogDebug("");
        }

        [Conditional("DEBUG")]
        public void LogDebug(string format, params object[] args)
        {
            var message = string.Format(format, args);

            _logger.LogDebug("{message}{Newline}", message, NewLine);
        }

        [Conditional("DEBUG")]
        public void LogHeader(string format, params object[] args)
        {
            var message = string.Format(format, args).Trim();

            var paddedMessage =
                (message + " ").PadRight(HeaderLength, HeaderPaddingChar);

            LogBlankLine();

            LogInfo(paddedMessage + NewLine);

            LogBlankLine();
        }

        [Conditional("DEBUG")]
        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public void LogInfo(string format, params object[] args)
        {
            var message = $"INFORMATION:  {string.Format(format, args)}";

            _logger.LogInformation("{message}", message);
        }

        [Conditional("DEBUG")]
        public void LogObjectAsJson<T>(object obj,
                // Avoid extra if-checks when skipping similar conditions
                bool ignore = false, bool keepNulls = false)
        {
            try
            {
                if (ignore || obj is null)
                    return;

                var formattedJson = SerializeAndFormat<T>(obj, keepNulls);

                if (string.IsNullOrWhiteSpace(formattedJson))
                    return;

                _logger.LogTrace("{formattedJson}{NewLine}",
                    formattedJson, NewLine);
            }
            catch (Exception exception)
            {
                LogExceptionRouter(exception);
            }
        }

        [Conditional("DEBUG")]
        public void LogSubheader(string format, params object[] args)
        {
            var message = string.Format(format, args);

            LogBlankLine();

            LogInfo(message + NewLine);

            LogBlankLine();
        }

        [Conditional("DEBUG")]
        public void LogTrace(string format, params object[] args)
        {
            var message = string.Format(format, args);

            _logger.LogTrace("{message}{Newline}", message, NewLine);
        }

        [Conditional("DEBUG")]
        public void LogValue(string label, string value,
            bool newLineAfter = false)
        {
            var message = $"{label}{Indent}={Indent}{value}";
            if (newLineAfter)
                message += NewLine;

            _logger.LogTrace("{message}{Newline}", message, NewLine);
        }

        [Conditional("DEBUG")]
        public void LogWarning(string format, params object[] args)
        {
            var message = $"WARNING:  {string.Format(format, args)}";

            _logger.LogWarning("{message}{Newline}", message, NewLine);
        }

        #endregion

        #region Private

        private void LogCommonException(Exception exception, string outerLabel,
            string innerLabel = "INNER EXCEPTION")
        {
            LogError($"{outerLabel}:  {exception.Message}{NewLine}");

            var innerExceptionMessage = exception.InnerException?.Message;

            if (string.IsNullOrWhiteSpace(innerExceptionMessage))
                return;

            LogError($"{Indent}{innerLabel}:  {innerExceptionMessage}{NewLine}");
        }

        #endregion

    }
}