using CsvHelper.Configuration.Attributes;
using SessionizeApi.Models;
using System;
using System.Diagnostics;
using System.Linq;

namespace SessionizeApi.SocialMedia
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay) + ",nq}")]
    public class SpeakerThanks
    {
        public SpeakerThanks(Speaker speaker, Session session)
        {
            FullName = RemoveNamePlaceholders(speaker);
            Title = session.Title;
            FirstName = speaker.FirstName;
            TwitterHandle = FindTwitterHandle(speaker);
        }

        [Name("Speaker Full Name")]
        public string FullName { get; private set; }

        [Name("Session")]
        public string Title { get; private set; }

        // Set in caller after TwitterHandle determined to allow custom messages
        [Name("Twitter Message")]
        public string TwitterMessage { get; set; }

        [Name("Twitter Chars")]
        public int TwitterChars => TwitterMessage.Length;

        [Name("Speaker First Name")]
        public string FirstName { get; private set; }

        [Name("Twitter Handle")]
        public string TwitterHandle { get; private set; }

        private string RemoveNamePlaceholders(Speaker speaker)
        {
            if(speaker.LastName == "-" || string.IsNullOrWhiteSpace(speaker.LastName))
                return speaker.FirstName;
            else
                return speaker.FullName;
        }

        private string FindTwitterHandle(Speaker speaker)
        {
            var twitterHandle = speaker.Links
                .Where(link => link.LinkType == "Twitter")
                .Select(link => $"@{System.IO.Path.GetFileName(link.Url.AbsolutePath)}")
                .FirstOrDefault();

            return twitterHandle;
        }

        #region Debug

        private string DebugTwitterHandle => TwitterHandle ?? "no Twitter";

        [Ignore]
        public virtual string DebuggerDisplay =>
            $"{FullName} - {DebugTwitterHandle} - |{TwitterChars}| - {Title}";

        [Ignore]
        public virtual string LogDisplay =>
            $"{FullName,-30} - {DebugTwitterHandle,-16} - |{TwitterChars,3}| - {Title}";

        public override string ToString()
        {
            try
            {
                var str = LogDisplay + $"\n\t\t\t\t{TwitterMessage}\n";

                return str;
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);

                return DebuggerDisplay;
            }
        }

        #endregion
    }
}
