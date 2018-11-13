namespace Markdown
{
    /// <summary>
    ///     Описывает разделитель и алгоритм, согласно которому должен отработать автомат в TextParser
    ///     Для каждого разделителя создаем нового наследника этого класса.
    ///     Не интерфейс т.к. скорее всего получится сюда вынести общий код.
    /// </summary>
    public interface ILexerRule
    {
        Delimiter ProcessIncomingChar(int position, Delimiter previousDelimiter, out bool shouldRemovePrevious);
        string[] PossibleDelimitersStrings { get; }
        bool Check(char symbol);
        bool Check(Delimiter delimiter);
        Delimiter Escape(Delimiter delimiter, string text);
        bool IsValid(Delimiter delimiter, string text);
        Token GetToken(Delimiter delimiter, string text);
    }
}
