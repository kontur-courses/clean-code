namespace Markdown
{
    /// <summary>
    /// Интерфейс для парсеров, соответствующих типу символа
    /// </summary>
    public interface IParser
    {
        public string Parse(string text);
    }
}

