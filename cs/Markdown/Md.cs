namespace Markdown
{
    /// <summary>
    /// Публичный класс, который будет работать с другими приватными классами, скрывая реализацию и предоставляя интерфейс
    /// </summary>
    public static class Md
    {
        public static string Render(string  markdownText)
        {
            var inlineParser = new InlineParser();
            var parser = new MarkdownParser(inlineParser);

            var result = parser.Parse(markdownText);
        }
    }
}
