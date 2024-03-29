using System;
using System.Diagnostics;
using System.Text.Json.Serialization;
using SessionizeApi.Importer.Serialization;
using static System.Guid;

namespace SessionizeApi.Importer.Models
{
    [JsonConverter(typeof(IdConverter))]
    public struct Id
    {
        #region API Properties

        public uint? Number;

        public Guid? Guid;

        #endregion

        #region Read Operators

        public static implicit operator Id(uint number) =>
            new() { Number = number };

        public static implicit operator Id(Guid guid) =>
            new() { Guid = guid };

        public static implicit operator Id(string str) =>
            ConvertIdString(str);

        #endregion

        #region Formatting Methods

        public override string ToString()
        {
            var str = Number != null
                ? ((uint)Number).ToString()
                : Guid != null
                    ? ((Guid)Guid).ToString()
                    : "NULL";

            return str;
        }

        #endregion

        #region Conversion Methods

        private static Id ConvertIdString(string str)
        {
            if (uint.TryParse(str, out var number))
            {
                return new Id { Number = number };
            }

            if (TryParse(str, out var guid))
            {
                return new Id { Guid = guid };
            }

            Debug.WriteLine($"Id (string => empty) = '{str}'");

            return new Id();
        }

        #endregion
    }
}
