namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdownText)
        {
            var temp = new MarkdownTokenizer().SplitTextToTokens(markdownText);
            
            var htmlFormattedText = TokenConverter.ConvertToHtml(temp);
            
            return htmlFormattedText;
        }
    }
}
