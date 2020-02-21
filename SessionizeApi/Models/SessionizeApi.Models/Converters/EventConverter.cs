using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Globalization;

namespace SessionizeApi.Models.Converters
{
    public static class EventConverter
    {
        public static readonly JsonSerializerSettings Settings =
            new JsonSerializerSettings
            {
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                DateParseHandling = DateParseHandling.None,
                Converters =
                {
                    IdConverter.Singleton,
                    new IsoDateTimeConverter
                        {DateTimeStyles = DateTimeStyles.AssumeUniversal}
                },
            };
    }
}
