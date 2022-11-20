using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.HtmlTag;
using Markdown.Tokens;

namespace Markdown.Markdown
{
    static class Md
    {
        public static string Render(string MarkdownString)
        {
            var tokenList = MarkdownParser.GetArrayWithMdTags(MarkdownString).OrderBy(tag => tag.Position).ToList();
            var tokens = TokenParser.GetTokens(tokenList, MarkdownString.Length);
            tokens = tokens.Filter(MarkdownString);
            var htmlString = HtmlParser.Parse(tokens, MarkdownString);
            return htmlString;
        }
    }
}
