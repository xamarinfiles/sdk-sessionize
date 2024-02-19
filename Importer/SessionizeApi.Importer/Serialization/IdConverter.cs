using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using SessionizeApi.Importer.Models;
using static SessionizeApi.Importer.Constants.Characters;

namespace SessionizeApi.Importer.Serialization
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
        // Read converter is skipped due to implicit operators on incoming Ids
        public override Id Read(ref Utf8JsonReader reader, Type typeToConvert,
            JsonSerializerOptions options)
        {
            throw new NotImplementedException();
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
