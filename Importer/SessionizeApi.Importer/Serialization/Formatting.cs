using System;
using System.Diagnostics;
using System.Text.Json;
using static SessionizeApi.Importer.Serialization.Configurations;

namespace SessionizeApi.Importer.Serialization
{
    public static class Formatting
    {
        public static string
            SerializeAndFormat<T>(object obj, bool keepNulls = false)
        {
            if (obj is null)
                // TODO Case where need to return message about null object?
                return "";

            T typedObject;

            if (obj is string str)
            {
                try
                {
                    var deserializedObject =
                        JsonSerializer.Deserialize<T>(str, ParseJsonOptions);

                    typedObject = deserializedObject != null
                        ? deserializedObject
                        // Could also be NotSupportedException if JsonConverter
                        : throw new JsonException();
                }
                catch (Exception)
                {
                    var debugMessage =
                        $"Unable to deserialize object of type {nameof(T)}: \"{str}\"";

                    return debugMessage;
                }
            }
            else
            {
                try
                {
                    typedObject = (T)obj;
                }
                catch (Exception)
                {
                    var debugMessage =
                        $"Unable to cast object to type {nameof(T)}: \"{obj}\"";

                    return debugMessage;
                }
            }

            var debugSerializerOptions = GetDebugJsonOptions(keepNulls);

            try
            {
                var formattedJson =
                    JsonSerializer.Serialize(typedObject, debugSerializerOptions);

                return formattedJson;
            }
            catch (Exception exception)
            {
                var exceptionMessage  =
                    JsonSerializer.Serialize(exception, debugSerializerOptions);

                Debug.WriteLine(exceptionMessage);

                var debugMessage =
                    $"Unable to serialize object of type {nameof(T)}: \"{typedObject}\"";

                return debugMessage;
            }
        }

    }
}
