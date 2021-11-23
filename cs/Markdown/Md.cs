namespace Markdown
{
    public static class Md
    {
        public static string Render(string markdownText)
        {
            var tokens = Tokenizer.ProcessMarkdown(markdownText);
            return tokens.Render();
        }
    }
}
