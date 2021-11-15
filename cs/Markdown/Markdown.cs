using Markdown.Tokens;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string mdText)
        {
            var tokens = new TokenReader(mdText, new ItalicToken()).FindAll();
            return new MarkdownRenderer(mdText).RenderMatches(tokens);
        }
    }
}