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
            try
            {
                switch(sort)
                {
                    case CustomSort.Unsorted:
                        break;
                    case CustomSort.CollectSpeakerSessionsByFirstSubmission:
                        var speakerSessions =
                            @event.Sessions.Where(session => session.SpeakerIds?.Length > 0);
                        var groupedSessions =
                            speakerSessions.GroupBy(session => session.SpeakerIds.First());
                        var reorderedSessions =
                            groupedSessions.SelectMany(session => session).ToArray();

                        @event.Sessions = reorderedSessions;
                        break;
                }

                return @event;
            }
            catch(System.Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
