using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SessionizeApi.Importer.Serialization
{
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class Configurations
    {
        #region JSON Serialization

        public static readonly JsonSerializerOptions
            DefaultReadJsonOptions = new()
            {
                AllowTrailingCommas = true,
                NumberHandling = JsonNumberHandling.AllowReadingFromString,
                PropertyNameCaseInsensitive = true
            };

        // NOTE Not working for apostrophes in Sessionize sample data
        //public static readonly JavaScriptEncoder
        //    DefaultWriteJavaScriptEncoder =
        //        JavaScriptEncoder.Create(
        //            new TextEncoderSettings(UnicodeRanges.All));

        public static readonly JsonSerializerOptions
            DefaultWriteJsonOptionsWithNull = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.Never,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

        public static readonly JsonSerializerOptions
            DefaultWriteJsonOptionsWithoutNull = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                // TODO Get this to work with standard JavaScriptEncoder
                // HACK Fix for apostrophes in Sessionize sample data
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

        #endregion
    }
}
