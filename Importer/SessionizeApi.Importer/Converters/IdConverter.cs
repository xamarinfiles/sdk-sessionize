using SessionizeApi.Importer.Models;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using static SessionizeApi.Importer.Constants.Characters;

namespace SessionizeApi.Importer.Converters
{
    // Id fields in Sessionize data can take a variety of formats (from sample event):
    //
    // Session:
    //  {
    //      "id": "14022",
    //      ...
    //      "speakers": [
    //          "00000000-0000-0000-0000-000000000004"
    //      ],
    //      "categoryItems": [
    //          4373,
    //          ...
    //      ],
    //      "questionAnswers": [
    //          {
    //              "questionId": 148,
    //              ...
    //          }
    //      ],
    //      "roomId": 215,
    //      ...
    //  }
    //
    // Speaker:
    //  {
    //      "id": "00000000-0000-0000-0000-000000000004",
    //      ...
    //      "sessions": [
    //          14022
    //      ],
    //      ...
    //  }

    public class IdConverter : JsonConverter<Id>
    {
        [SuppressMessage("ReSharper", "InvertIf")]
        [SuppressMessage("ReSharper", "SwitchStatementHandlesSomeKnownEnumValuesWithDefault")]
        public override Id Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            try
            {
                switch (reader.TokenType)
                {
                    // Unusual match is due to variety of Sessionize id formats
                    case JsonTokenType.String:
                        var idStr = reader.GetString();

                        if (uint.TryParse(idStr, out var result))
                        {
                            // Debug.WriteLine($"Id (string => number) = '{idStr}'");

                            return new Id { Number = result };
                        }

                        if (Guid.TryParse(idStr, out var strGuid))
                        {
                            // Debug.WriteLine($"Id (string => guid) = '{idStr}'");

                            return new Id { Guid = strGuid };
                        }

                        // Debug.WriteLine($"Id (string => empty) = '{idStr}'");

                        return new Id();
                    case JsonTokenType.Number
                        when reader.TryGetUInt32(out var idNum):

                        //Debug.WriteLine($"Id (number) = {idNum}");

                        return new Id { Number = idNum };
                    default:
                        //var input =
                        //    JsonDocument.ParseValue(ref reader).RootElement.Clone();

                        //Debug.WriteLine(
                        //    $"Id ({reader.TokenType} => empty) = '{input}'");

                        return new Id();
                }
            }
            catch (Exception exception)
            {
                var unrecognizedJson =
                    JsonDocument.ParseValue(ref reader).RootElement.Clone();

                Debug.WriteLine($"Unrecognized Id = {unrecognizedJson}{NewLine}");

                Debug.WriteLine($"EXCEPTION:  {exception.Message}{NewLine}");

                var innerExceptionMessage = exception.InnerException?.Message;

                if (!string.IsNullOrWhiteSpace(innerExceptionMessage))
                    Debug.WriteLine(
                        $"{Indent}INNER EXCEPTION:  {innerExceptionMessage}{NewLine}");

                return new Id();
            }
        }

        public override void Write(Utf8JsonWriter writer, Id value,
            JsonSerializerOptions options)
        {
            if (value.Number != null)
            {
                writer.WriteStringValue(value.Number.ToString());
            }
            else if (value.Guid != null)
            {
                writer.WriteStringValue(value.Guid.ToString());
            }
        }
    }
}
