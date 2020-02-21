using System;

namespace SessionizeApi.Models
{
    public partial struct Id
    {
        public int? Integer;
        public Guid? Uuid;

        public static implicit operator Id(int Integer) => new Id { Integer = Integer };
        public static implicit operator Id(Guid Uuid) => new Id { Uuid = Uuid };

        public override string ToString()
        {
            if(Integer != null)
                return Integer.ToString();

            return Integer != null ? Integer.ToString() : Uuid.ToString();
        }
    }
}
