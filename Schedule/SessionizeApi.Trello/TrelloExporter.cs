using Manatee.Trello;
using SessionizeApi.Importer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using XamarinFiles.FancyLogger;
using static Manatee.Trello.LabelColor;
using TrelloIList = Manatee.Trello.IList;

namespace SessionizeApi.Trello
{
    // TODO Fix custom categories with "&" or split into separate
    [SuppressMessage("ReSharper", "ArrangeObjectCreationWhenTypeEvident")]
    public class TrelloExporter
    {
        #region Constants

        private const string AcceptedListName = "Accepted Sessions";

        private const string UnselectedListName = "Unselected Sessions";

        #endregion

        #region Services
        private IFancyLogger FancyLogger { get; }

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

        #region Constructor

        public TrelloExporter(IFancyLogger fancyLogger, string apiKey,
            string apiToken, LabelColor defaultLabelColor = Black)
        {
            FancyLogger = fancyLogger;
            ApiKey = apiKey;
            ApiToken = apiToken;
            DefaultLabelColor = defaultLabelColor;
        }

        #endregion

        #region Trello Methods

        public async Task ConnectToTrelloAndLoadBoard(Event @event, string trelloBoardId)
        {
            SessionizeEvent = @event;
            BoardId = trelloBoardId;

            // TODO BuildSessionizeCategoryDictionary();

            SetupTrelloUserCredentials();

            Board = await ConnectToTrelloBoard();

            await LoadTrelloBoard();
        }

        private void SetupTrelloUserCredentials()
        {
            try
            {
                TrelloAuthorization.Default.AppKey = ApiKey;
                TrelloAuthorization.Default.UserToken = ApiToken;
            }
            catch (Exception exception)
            {
                FancyLogger.LogException(exception);
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
                FancyLogger.LogException(exception);

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

                    FancyLogger.LogHeader("{0}: {1}",
                        args: new object[] { speakerIndex, speaker.LogDisplayShort });

                    foreach (var sessionIdAndName in speaker.SessionReferences)
                    {
                        FancyLogger.LogHeader("{0}: {1}",
                            args: new object[] { speakerIndex, sessionIdAndName.LogDisplayShort });

                        var session =
                            SessionizeEvent.SessionDictionary[sessionIdAndName.Id];
                        if (session is null)
                        {
                            continue;
                        }

                        var categories = session.CategoryReferences;
                        await LoadTrelloCard(
                            categories?.Count() < 1 ? unselectedList : acceptedList,
                            session);
                    }
                }

                await acceptedList.Refresh();
                var acceptedCount = acceptedList.Cards.Count();
                FancyLogger.LogScalar("Accepted Sessions", acceptedCount.ToString());

                await unselectedList.Refresh();
                var unselectedCount = unselectedList.Cards.Count();
                FancyLogger.LogScalar("Unselected Sessions", unselectedCount.ToString());

                var totalCount = acceptedCount + unselectedCount;
                FancyLogger.LogScalar("Total Sessions", totalCount.ToString());
            }
            catch (Exception exception)
            {
                FancyLogger.LogException(exception);
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
            FancyLogger.LogScalar(trelloList.Name, session.Title);

            try
            {
                var cardName =
                    $"{session.Id} - {session.SpeakerNames} - {session.Title}";
                var cardDescription = session.Description;

                // TEMP
                FancyLogger.LogScalar("Card Name", cardName);
                FancyLogger.LogScalar("Card Description", cardDescription);

                var sessionCategories =
                    session.CategoryReferences
                        .Select(category => category.Name)
                        .ToList();

                var cardLabels = new List<ILabel>();
                foreach (var category in sessionCategories)
                {
                    //// TODO Still need for OCC 2023?
                    //// TEMP HACK until fix categories in Sessionize
                    //string adjustedCategory;
                    //switch (category)
                    //{
                    //    case "AI & ML":
                    //        adjustedCategory = "AI / ML";
                    //        break;
                    //    case "Tools & IDEs":
                    //        adjustedCategory = "Tools / IDEs";
                    //        break;
                    //    default:
                    //        adjustedCategory = category;
                    //        break;
                    //}

                    // var label = await LoadTrelloLabel(adjustedCategory);

                    var label = await LoadTrelloLabel(category);

                    FancyLogger.LogScalar(category, label.Id);

                    cardLabels.Add(label);
                }

                if (cardLabels.Count > 0)
                    // TEMP
                    FancyLogger.LogScalar("Card Labels",
                        string.Join(", ",
                            cardLabels.Select(label => label.Name).ToList()));

                await trelloList.Refresh();

                // TODO Figure out why this is duplicating first sample session
                if (trelloList.Cards
                    .Any(card => card.Name.StartsWith(session.Id.ToString())))
                {
                    FancyLogger.LogWarning("DUPLICATE");

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
                FancyLogger.LogException(exception);
            }
        }

        #endregion
    }
}
