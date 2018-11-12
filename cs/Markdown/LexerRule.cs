namespace Markdown
{
    /// <summary>
    ///     Описывает разделитель и алгоритм, согласно которому должен отработать автомат в TextParser
    ///     Для каждого разделителя создаем нового наследника этого класса.
    ///     Не интерфейс т.к. скорее всего получится сюда вынести общий код.
    /// </summary>
    public interface ILexerRule
    {
        string Delimiter { get; }
        Delimiter ProcessIncomingChar(char incomingChar, Delimiter previousDelimiter);

        bool Check(char symbol);
    }
}
