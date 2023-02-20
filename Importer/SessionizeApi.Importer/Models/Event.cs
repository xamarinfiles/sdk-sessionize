using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SessionizeApi.Importer.Models
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class Event : AllData
    {
        #region Constructors

        private Event(AllData allData)
        {
            Sessions = allData.Sessions
                // TEMP
                .OrderBy(session => session.Title)
                .ToArray();
            Speakers = allData.Speakers
                // TEMP
                .OrderBy(speaker => speaker.FullName)
                .ToArray();
            Questions = allData.Questions;
            Categories = allData.Categories;
            Rooms = allData.Rooms;
            DebuggerDisplay = allData.DebuggerDisplay;
            LogDisplayShort = allData.LogDisplayShort;
            LogDisplayLong = allData.LogDisplayLong;

            PopulateDependencyDictionaries();

            FormatDependentFields();
        }

        public static Event Create(AllData allData)
        {
            var @event = new Event(allData);

            return @event;
        }

        #endregion

        #region Dependency Properties

        public IDictionary<Id, Session> SessionDictionary { get; set; }

        public IDictionary<Id, Speaker> SpeakerDictionary { get; set; }

        public IDictionary<Id, Item> CategoryDictionary { get; set; }

        #endregion

        #region Dependency Methods

        private void PopulateDependencyDictionaries()
        {
            SessionDictionary =
                Sessions.ToDictionary(session => session.Id);

            SpeakerDictionary =
                Speakers.ToDictionary(speaker => speaker.Id);

            CategoryDictionary = new Dictionary<Id, Item>();
            foreach (var (item, itemId) in
                     Categories
                         .SelectMany(
                             category =>
                                 category.Items.Select(item => (item, item.Id))))
            {
                CategoryDictionary[itemId] = item;
            }
        }

        private void FormatDependentFields()
        {
            foreach (var session in Sessions)
            {
                session.FormatDependentFields(SpeakerDictionary, CategoryDictionary);
            }

            foreach (var speaker in Speakers)
            {
                speaker.FormatDependentFields(SessionDictionary, CategoryDictionary);
            }
        }

        #endregion
    }
}
