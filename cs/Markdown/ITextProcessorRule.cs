namespace Markdown
{
    /// <summary>
    ///     Describes set of rules for new tags
    /// </summary>
    internal interface ITextProcessorRule
    {
        bool Check(int position, string text);

        bool Check(Delimiter delimiter);

        Delimiter Escape(Delimiter delimiter, string text);

        Token GetToken(Delimiter delimiter, string text);

        bool IsValid(Delimiter delimiter, string text);

        bool IsValidAsOpening(Delimiter delimiter, string text);

        bool IsValidClosing(Delimiter delimiter, string text);

        Delimiter ProcessIncomingChar(int position, string text, out int amountOfSymbolsToSkip);
    }
}
