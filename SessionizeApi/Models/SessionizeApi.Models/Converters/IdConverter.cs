using Newtonsoft.Json;
using System;

namespace SessionizeApi.Models.Converters
{
    internal class IdConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Id) || t == typeof(Id?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.String:
                case JsonToken.Date:
                    var stringValue = serializer.Deserialize<string>(reader);
                    int integer;
                    if (int.TryParse(stringValue, out integer))
                    {
                        return new Id { Integer = integer };
                    }
                    Guid guid;
                    if (Guid.TryParse(stringValue, out guid))
                    {
                        return new Id { Uuid = guid };
                    }
                    break;
            }
            throw new Exception("Cannot unmarshal type Id");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            var value = (Id)untypedValue;
            if (value.Integer != null)
            {
                serializer.Serialize(writer, value.Integer.Value.ToString());
                return;
            }
            if (value.Uuid != null)
            {
                serializer.Serialize(writer, value.Uuid.Value.ToString("D", System.Globalization.CultureInfo.InvariantCulture));
                return;
            }
            throw new Exception("Cannot marshal type Id");
        }

        public static readonly IdConverter Singleton = new IdConverter();
    }
}
