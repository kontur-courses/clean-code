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
            var tokenList = MarkdownParser.GetListWithMdTags(markdownString).OrderBy(tag => tag.Position).ToList();
            var tokens = TokenParser.GetTokens(tokenList, markdownString.Length);
            tokens = tokens.Filter(markdownString);
            var htmlString = HtmlParser.Parse(tokens, markdownString);
            return htmlString;
        }
    }
}
