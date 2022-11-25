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
            var markdownTokens = MarkdownParser.GetArrayWithMdTags(markdownString);
            var tokens = TokenParser.GetTokens(markdownTokens, markdownString.Length);
            return HtmlParser.Parse(tokens.Filter(markdownString), markdownString);
        }
    }
}
