using System;
using System.Diagnostics;

namespace SessionizeApi.Importer.Models
{
    public struct Id
    {
        #region API Properties

        public uint? Number;

        public Guid? Guid;

        #endregion

        #region Load Operators

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
            return Number != null
                ? ((uint)Number).ToString()
                : Guid != null
                    ? ((Guid)Guid).ToString()
                    : "NULL";
        }

        #endregion

        #region Conversion Methods

        private static Id ConvertIdString(string str)
        {
            if (uint.TryParse(str, out var number))
            {
                return new Id { Number = number };
            }

            if (System.Guid.TryParse(str, out var guid))
            {
                return new Id { Guid = guid };
            }

            Debug.WriteLine($"Id (string => empty) = '{str}'");

            return new Id();
        }

        #endregion
    }
}
