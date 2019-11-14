using System.Collections.Generic;
using System.Text;
using Markdown.Core.HTMLTags;
using Markdown.Core.Tokens;

namespace Markdown.Core
{
    public class MdToHtmlTranslator
    {
        private readonly Dictionary<string, string> tagDict = new Dictionary<string, string>()
        {
            {"__", "strong"},
            {"_", "em"}
        };

        public string TranslateTokensToHtml(IEnumerable<IToken> tokens)
        {
            var result = new StringBuilder();
            foreach (var token in tokens)
            {
                result.Append(TranslateOneTokenToHtml(token));
            }
            return result.ToString();
        }

        private string TranslateOneTokenToHtml(IToken token)
        {
            if (token.TokenType == TokenType.HTMLTag)
            {
                var tag = token as HTMLTagToken;
                var tagName = tagDict[tag.Value];
                return tag.IsOpen ? $"<{tagName}>" : $"</{tagName}>";
            }
            return token.Value;
        }
    }
}