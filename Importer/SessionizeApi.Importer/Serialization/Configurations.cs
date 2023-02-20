using SessionizeApi.Importer.Converters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SessionizeApi.Importer.Serialization
{
    internal static class Configurations
    {
        #region JSON Serialization

        public static JsonSerializerOptions
            GetDebugJsonOptions(bool keepNulls)
        {
            return keepNulls ? DebugJsonOptionsWithNull : DebugJsonOptionsWithoutNull;
        }

        public static readonly JsonSerializerOptions
            DebugJsonOptionsWithNull = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                Converters = { new IdConverter() },
                WriteIndented = true
            };

        public static readonly JsonSerializerOptions
            DebugJsonOptionsWithoutNull = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Converters = { new IdConverter() },
                WriteIndented = true
            };

        public static readonly JsonSerializerOptions
            ParseJsonOptions = new()
            {
                AllowTrailingCommas = true,
                Converters = { new IdConverter() },
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true
            };

        #endregion
    }
}
