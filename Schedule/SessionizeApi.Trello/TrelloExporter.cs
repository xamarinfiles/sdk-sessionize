using Manatee.Trello;
using SessionizeApi.Importer.Logger;
using SessionizeApi.Importer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using static Manatee.Trello.LabelColor;
using TrelloIList = Manatee.Trello.IList;

namespace SessionizeApi.Trello
{
    [SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeEvident")]
    public class TrelloExporter
    {
        #region Constants

        private const string AcceptedListName = "Accepted Sessions";

        private const string UnselectedListName = "Unselected Sessions";

        #endregion

        #region Services

        private static LoggingService LoggingService { get; set; }

        #endregion

        #region Sessionize Properies

        private Event SessionizeEvent { get; set; }

        #endregion

        #region Trello Properties

        private static string ApiKey { get; set; }

        private static string ApiToken { get; set; }

        private static LabelColor DefaultLabelColor { get; set; }

        private readonly TrelloFactory _trelloFactory = new TrelloFactory();

        private string BoardId { get; set; }

        private IBoard Board { get; set; }

        #endregion

        #region Trello Methods

        public TrelloExporter(LoggingService loggingService, string apiKey,
            string apiToken, LabelColor defaultLabelColor = Black)
        {
            LoggingService = loggingService;
            ApiKey = apiKey;
            ApiToken = apiToken;
            DefaultLabelColor = defaultLabelColor;
        }

        public async Task ConnectToTrelloAndLoadBoard(Event @event, string trelloBoardId)
        {
            SessionizeEvent = @event;
            BoardId = trelloBoardId;

            // TODO BuildSessionizeCategoryDictionary();

            SetupTrelloUserCredentials();

            Board = await ConnectToTrelloBoard();

            await LoadTrelloBoard();
        }

        private static void SetupTrelloUserCredentials()
        {
            try
            {
                TrelloAuthorization.Default.AppKey = ApiKey;
                TrelloAuthorization.Default.UserToken = ApiToken;
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);
            }
        }

        private async Task<IBoard> ConnectToTrelloBoard()
        {
            try
            {
                var board = _trelloFactory.Board(BoardId);
                await board.Refresh();

                return board;
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);

                return null;
            }
        }
        private async Task LoadTrelloBoard()
        {
            try
            {
                // TODO Group a user and all their sessions, sorted by first submission
                var sortedSpeakers =
                    SessionizeEvent.Speakers.OrderBy(speaker => speaker.FullName).ToList();

                var acceptedList = await LoadTrelloList(AcceptedListName);
                var unselectedList = await LoadTrelloList(UnselectedListName);

                for (var speakerIndex = 0;
                     speakerIndex < SessionizeEvent.Speakers.Length;
                     speakerIndex++)
                {
                    var speaker = sortedSpeakers[speakerIndex];

                    LoggingService.LogHeader("{0}: {1}", speakerIndex,
                        speaker.LogDisplayShort);

                    foreach (var sessionIdAndName in speaker.SessionIdsAndNames)
                    {
                        LoggingService.LogSubheader("{0}: {1}", speakerIndex,
                            sessionIdAndName.LogDisplayShort);

                        var session =
                            SessionizeEvent.SessionDictionary[sessionIdAndName.Id];
                        if (session is null)
                        {
                            continue;
                        }

                        // HACK OCC 2023 only categorized accepted sessions
                        var categories = session.CategoryIdsAndNames;
                        await LoadTrelloCard(
                            categories?.Count() < 1 ? unselectedList : acceptedList,
                            session);
                    }

                    LoggingService.LogBlankLine();
                }

                await acceptedList.Refresh();
                var acceptedCount = acceptedList.Cards.Count();
                LoggingService.LogValue("Accepted Sessions", acceptedCount.ToString());

                await unselectedList.Refresh();
                var unselectedCount = unselectedList.Cards.Count();
                LoggingService.LogValue("Unselected Sessions", unselectedCount.ToString());

                var totalCount = acceptedCount + unselectedCount;
                LoggingService.LogValue("Total Sessions", totalCount.ToString());
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);
            }
        }

        private async Task<TrelloIList> LoadTrelloList(string listName)
        {
            switch (Board.Lists.FirstOrDefault(list => list.Name.Contains(listName)))
            {
                case { } oldList:
                    return oldList;
                default:
                {
                    var newList = await Board.Lists.Add(listName);

                    return newList;
                }
            }
        }

        private async Task<ILabel> LoadTrelloLabel(string labelName)
        {
            switch (Board.Labels.FirstOrDefault(label => label.Name.Contains(labelName)))
            {
                case { } oldLabel:
                    return oldLabel;
                default:
                {
                    // TODO Support more than one color? Random? Sequential?
                    var newLabel = await Board.Labels.Add(labelName, DefaultLabelColor);

                    return newLabel;
                }
            }
        }

        private async Task LoadTrelloCard(TrelloIList trelloList, Session session)
        {
            // TEMP
            LoggingService.LogValue(trelloList.Name, session.Title);

            try
            {
                var cardName =
                    $"{session.Id} - {session.SpeakerNames} - {session.Title}";
                var cardDescription = session.Description;

                // TEMP
                LoggingService.LogValue("Card Name", cardName);
                LoggingService.LogValue("Card Description", cardDescription);

                var sessionCategories =
                    session.CategoryIdsAndNames
                        .Select(category => category.Name)
                        .ToList();

                var cardLabels = new List<ILabel>();
                foreach (var category in sessionCategories)
                {
                    var label = await LoadTrelloLabel(category);

                    LoggingService.LogValue(category, label.Id);

                    cardLabels.Add(label);
                }

                if (cardLabels.Count > 0)
                    // TEMP
                    LoggingService.LogValue("Card Labels",
                        string.Join(", ",
                            cardLabels.Select(label => label.Name).ToList()));

                await trelloList.Refresh();

                // TODO Figure out why this is duplicating first sample session
                if (trelloList.Cards
                    .Any(card => card.Name.StartsWith(session.Id.ToString())))
                {
                    LoggingService.LogWarning("DUPLICATE");

                    return;
                }

                await trelloList.Cards.Add(
                    name: cardName,
                    description: cardDescription,
                    labels: cardLabels
                );
            }
            catch (Exception exception)
            {
                LoggingService.LogExceptionRouter(exception);
            }
        }

        #endregion
    }
}
