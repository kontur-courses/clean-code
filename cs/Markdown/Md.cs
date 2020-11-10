namespace Markdown
{
    public class Md
    {
        public string Render(string text)
        {
            var markdownParser = new MarkdownParser(text);
            markdownParser.Parse();
            text = markdownParser.TextWithoutEscapeCharacters;
            return MarkdownToHtmlConverter.Convert(text, markdownParser.Tags);
        }
    }
}