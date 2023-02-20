using System;

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
    }
}
