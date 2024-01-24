﻿using SessionizeApi.Importer.Dtos;
using SessionizeApi.Importer.Logger;
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
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
    public class Category : ILogFormattable
    {
        #region Constructor

        private Category(CategoryDto categoryDto,
            IFancyLogger fancyLogger)
        {
            Id = categoryDto.Id;
            Title = categoryDto.Title;
            Items = categoryDto.Items
                .Select(itemDto => Item.Create(itemDto,
                    fancyLogger))
                .ToArray();
            Sort = categoryDto.Sort;
            Type = categoryDto.Type;
        }

        public static Category Create(CategoryDto categoryDto,
            IFancyLogger fancyLogger)
        {
            try
            {
                var category = new Category(categoryDto, fancyLogger);

                return category;
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

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("items")]
        public Item[] Items { get; set; }

        [JsonPropertyName("sort")]
        public uint Sort { get; set; }

        // TODO Convert to CategoryType enum
        [JsonPropertyName("type")]
        public string Type { get; set; }

        #endregion

        #region Formatted Properties

        [JsonIgnore]
        public string DebuggerDisplay => $"{Id} - {Sort} - {Title} - {Type}";

        [JsonIgnore]
        public string LogDisplayShort => $"{Id,-3} - {Sort,-3} - {Title} - {Type}";

        // TODO
        [JsonIgnore]
        public string LogDisplayLong => LogDisplayShort;

        #endregion
    }
}
