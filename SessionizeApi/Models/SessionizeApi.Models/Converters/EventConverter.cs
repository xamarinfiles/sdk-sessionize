using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace SessionizeApi.Models.Converters
{
    internal static class EventConverter
    {
        internal static readonly JsonSerializerSettings Settings =
            new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
                {
                    // DEL IdConverter.Singleton,
                    new IsoDateTimeConverter
                        {DateTimeStyles = DateTimeStyles.AssumeUniversal}
                },
            };
    }
}
