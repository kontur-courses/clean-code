using System.Globalization;

namespace Markdown
{
    /// <summary>
    /// Публичный класс, который будет работать с другими приватными классами, скрывая реализацию и предоставляя интерфейс
    /// </summary>
    public static class Md
    {
        public static string FromMarkdownToHtml(string  markdownText)
        {
             throw new NotImplementedException();
        }
    }

    internal class MarkdownParser
    {
        private readonly InlineParser inlineParser;

        public string Parse(string markdownText)
        {
            throw new NotImplementedException();
        }
    }
}
