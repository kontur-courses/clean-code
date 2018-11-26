namespace Markdown
{
    /// <summary>
    ///     Описывает разделитель и алгоритм, согласно которому должен отработать автомат в TextParser
    ///     Для каждого разделителя создаем нового наследника этого класса.
    /// </summary>
    internal interface ILexerRule
    {
        Delimiter ProcessIncomingChar(int position, string text, out int amountOfSymbolsToSkip);
        bool Check(int position, string text);
        bool Check(Delimiter delimiter);
        Delimiter Escape(Delimiter delimiter, string text);
        bool IsValid(Delimiter delimiter, string text);
        Token GetToken(Delimiter delimiter, string text);
        bool IsValidFirst(Delimiter delimiter, string text);
        bool IsValidSecond(Delimiter delimiter, string text);
    }
}
