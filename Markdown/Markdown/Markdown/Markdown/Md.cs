using Markdown.HtmlTag;
using Markdown.Tokens;

namespace Markdown.Markdown
{
    public static class Md
    {
        public static string Render(string markdownString)
        {
            if (string.IsNullOrEmpty(markdownString))
                throw new ArgumentNullException("String for render must not be null or empty");
            var markdownTokens = MarkdownParser.GetListWithMdTags(markdownString).OrderBy(token => token.Position).ToList();
            var tokens = TokenParser.GetTokens(markdownTokens, markdownString.Length);
            tokens = tokens.Filter(markdownString);
            return HtmlParser.Parse(tokens, markdownString);
        }
    }
}
