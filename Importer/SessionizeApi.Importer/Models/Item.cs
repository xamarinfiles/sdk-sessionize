using System;
using SessionizeApi.Importer.Logger;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using SessionizeApi.Importer.Dtos;
using XamarinFiles.FancyLogger;

namespace SessionizeApi.Importer.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [SuppressMessage("ReSharper",
        "AutoPropertyCanBeMadeGetOnly.Global")]
    [SuppressMessage("ReSharper",
        "ClassNeverInstantiated.Global")]
    [SuppressMessage("ReSharper",
        "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper",
        "UnusedAutoPropertyAccessor.Global")]
    public class Item : ILogFormattable
    {
        #region Constructor

        private Item(ItemDto itemDto)
        {
            Id = itemDto.Id;
            Name = itemDto.Name;
            Sort = itemDto.Sort;
        }

        private Item(Id id, string name, uint sort)
        {
            Id = id;
            Name = name;
            Sort = sort;
        }

        public static Item Create(ItemDto itemDto,
            IFancyLogger fancyLogger)
        {
            try
            {
                var item = new Item(itemDto);

                return item;
            }
            catch (Exception exception)
            {
                fancyLogger.LogException(exception);

                return null;
            }
        }

        public static Item Create(Id id, string name, uint sort,
            IFancyLogger fancyLogger)
        {
            try
            {
                var item = new Item(id, name, sort);

                return item;
            }
            catch (Exception exception)
            {
                fancyLogger.LogException(exception);

                return null;
            }
        }

        #endregion

        #region Original and Replacement API Properties

        [JsonPropertyName("id")]
        public Id Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("sort")]
        public uint Sort { get; set; }

        #endregion

        #region Formatted Properties

        [JsonIgnore]
        public string DebuggerDisplay => $"{Id} - {Sort} - {Name}";

        [JsonIgnore]
        public string LogDisplayShort => $"{Id,-3} - {Sort,-3} - {Name}";

        // TODO
        [JsonIgnore]
        public string LogDisplayLong => LogDisplayShort;

        #endregion
    }
}
