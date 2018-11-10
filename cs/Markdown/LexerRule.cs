namespace Markdown
{
    /// <summary>
    /// Описывает разделитель и алгоритм, согласно которому должен отработать автомат в TextParser
    /// Для каждого разделителя создаем нового наследника этого класса.
    /// Не интерфейс т.к. скорее всего получится сюда вынести общий код.
    /// </summary>
    public abstract class LexerRule
    {
        public abstract string DelimiterChar { get; }
        public abstract bool ProcessIncomingChar(string incomingChar);
    }
}