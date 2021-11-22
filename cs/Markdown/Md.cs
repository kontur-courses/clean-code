namespace Markdown
{
    public static class Md
    {
        public static string Render(string markdownText)
        {
            var tokenizer = new Tokenizer();
            var tokens = tokenizer.ProcessMarkdown(markdownText);
            return tokens.Render();
        }
    }
}
