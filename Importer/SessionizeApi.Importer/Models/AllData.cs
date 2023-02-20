using SessionizeApi.Importer.Logger;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using static SessionizeApi.Importer.Serialization.Configurations;

namespace SessionizeApi.Importer.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class AllData : ILogFormattable
    {
        #region API Properties

        [JsonPropertyName("sessions")]
        public Session[] Sessions { get; set; }

        [JsonPropertyName("speakers")]
        public Speaker[] Speakers { get; set; }

        [JsonPropertyName("questions")]
        public Question[] Questions { get; set; }

        [JsonPropertyName("categories")]
        public Category[] Categories { get; set; }

        [JsonPropertyName("rooms")]
        public Room[] Rooms { get; set; }

        #endregion

        #region Load Methods

        public static AllData FromJson(string json, LoggingService loggingService,
            string eventSource = null)
        {
            try
            {
                if (JsonSerializer.Deserialize<AllData>(json, ParseJsonOptions) is
                    { } allData)
                {
                    allData.FormatLogFields(eventSource, loggingService);

                    return allData;
                }
            }
            catch (Exception exception)
            {
                loggingService.LogExceptionRouter(exception);
            }

            loggingService.LogError($"Unable to import data from {eventSource}");

            return null;
        }

        #endregion

        #region Formatted Properties

        [JsonIgnore]
        public string DebuggerDisplay { get; protected set; }

        [JsonIgnore]
        public string LogDisplayShort { get; protected set; }

        [JsonIgnore]
        public string LogDisplayLong { get; protected set; }

        #endregion

        #region Formatting Methods

        private void FormatLogFields(string eventSource,
            LoggingService loggingService)
        {
            try
            {
                DebuggerDisplay = $"Source = {eventSource ?? string.Empty}";
                LogDisplayShort =
                    DebuggerDisplay +
                    $" - |Sessions| = {Sessions.Length} - |Speakers| = {Speakers.Length}";
                // TEMP
                LogDisplayLong = LogDisplayShort;
            }
            catch (Exception exception)
            {
                loggingService.LogExceptionRouter(exception);
            }
        }

        #endregion
    }
}
