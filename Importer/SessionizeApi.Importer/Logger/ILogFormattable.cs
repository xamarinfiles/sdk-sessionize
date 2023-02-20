namespace SessionizeApi.Importer.Logger
{
    internal interface ILogFormattable
    {
        string DebuggerDisplay { get; }

        string LogDisplayShort{ get; }

        string LogDisplayLong { get; }
    }
}
