using SessionizeApi.Importer.Logger;
using System;
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
    public class Link : ILogFormattable
    {
        #region Constructor

        private Link(LinkDto linkDto)
        {
            Title = linkDto.Title;
            Url = linkDto.Url;
            LinkType = linkDto.LinkType;
        }

        public static Link Create(LinkDto linkDto,
            IFancyLogger fancyLogger)
        {
            try
            {
                var link = new Link(linkDto);

                return link;
            }
            catch (Exception exception)
            {
                fancyLogger.LogException(exception);

                return null;
            }
        }

        #endregion

        #region Original and Replacement API Properties

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("url")]
        public Uri Url { get; set; }

        // TODO Convert to enum
        [JsonPropertyName("linkType")]
        public string LinkType { get; set; }

        #endregion

        #region Formatted Properties

        [JsonIgnore]
        public string DebuggerDisplay => $"{Title}: {LinkType} - {Url}";

        [JsonIgnore]
        public string LogDisplayShort => DebuggerDisplay;

        // TODO
        [JsonIgnore]
        public string LogDisplayLong => LogDisplayShort;

        #endregion
    }
}
