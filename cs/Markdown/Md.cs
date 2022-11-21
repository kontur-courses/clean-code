using Markdown.Tokens;

namespace Markdown
{
    public static class Md
    {
        public static string Render(string markdown)
        {
            if (markdown == null)
                throw new ArgumentNullException();
            var lines = markdown.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                var tokenizer = new Tokenizer();
                var tokens = tokenizer.TokenizeLine(lines[i]);
                lines[i] = HtmlConverter.ConvertTokensToHtml(tokens);
            }

            return string.Join('\n', lines);
        }
    }
}
