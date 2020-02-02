using FancyLogger;
using SessionizeApi.Models;
using System.Linq;

namespace SessionizeApi.Importer.Events
{
    internal class OrlandoCodeCampSessionizeImporter : SessionizeImporter
    {
        #region Constructor

        internal OrlandoCodeCampSessionizeImporter(FancyLoggerService loggingService) :
            base(loggingService)
        {

        }

        #endregion

        #region Event Importer

        protected override Event TransformEvent(Event @event, CustomSort sort)
        {
            switch (sort)
            {
                case CustomSort.Unsorted:
                    break;
                case CustomSort.CollectSpeakerSessionsByFirstSubmission:
                    var groupedSessions =
                        @event.Sessions.GroupBy(session => session.SpeakerIds.First());
                    var reorderedSessions =
                        groupedSessions.SelectMany(session => session).ToArray();

                    @event.Sessions = reorderedSessions;
                    break;
            }

            return @event;
        }

        #endregion
    }
}
