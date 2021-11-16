namespace Markdown
{
    public class Md
    {
        public string Render(string markdownText)
        {
            var tokenizer = new HtmlTokenizer();
            return tokenizer.ProcessMarkdown(markdownText).ToHtmlText();
        }
    }
}
