using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Core.HTMLTags;
using Markdown.Core.Infrastructure;
using Markdown.Core.Tokens;

namespace Markdown.Core
{
    public class MdToHtmlTranslator
    {
        public string TranslateTokensToHtml(IEnumerable<IToken> tokens)
        {
            var result = new StringBuilder();
            var headerTag = GetHeaderTag(tokens);
            if (headerTag != null)
            {
                result.Append($"<{headerTag}>");
                tokens = tokens.Skip(1);
            }

            foreach (var token in tokens)
            {
                result.Append(TranslateOneTokenToHtml(token));
            }

            if (headerTag != null)
                result.Append($"</{headerTag}>");
            return result.ToString();
        }

        private string GetHeaderTag(IEnumerable<IToken> tokens)
        {
            var firstToken = tokens.First();
            if (firstToken.TokenType == TokenType.HTMLTag)
            {
                var firstTag = firstToken as HTMLTagToken;
                if (firstTag.TagType == HTMLTagType.Header)
                    return TagsUtils.GetTagNameByMdTag(firstTag.Value);
            }

            return null;
        }

        private string TranslateOneTokenToHtml(IToken token)
        {
            if (token.TokenType == TokenType.HTMLTag)
            {
                var tag = token as HTMLTagToken;
                var tagName = TagsUtils.GetTagNameByMdTag(tag.Value);
                return tag.TagType == HTMLTagType.Opening ? $"<{tagName}>" : $"</{tagName}>";
            }
            return token.Value;
        }
    }
}