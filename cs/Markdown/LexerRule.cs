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

        bool Check(char symbol);
    }

    public class UnderscoreRule : ILexerRule
    {
        public Delimiter ProcessIncomingChar(int position, Delimiter previousDelimiter, out bool shouldRemovePrevious)
        {
            shouldRemovePrevious = false;
            if (previousDelimiter != null && previousDelimiter.Position + 1 == position && previousDelimiter.Value == "_")
            {
                shouldRemovePrevious = true;
                return new Delimiter(true, "__", position - 1);
            }
            return new Delimiter(true, "_", position);
        }

        public bool Check(char symbol) => symbol == '_';
    }
}
