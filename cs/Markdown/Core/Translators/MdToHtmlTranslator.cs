using System.Collections.Generic;
using System.Linq;
using System.Text;
using Markdown.Core.HTMLTags;
using Markdown.Core.Infrastructure;
using Markdown.Core.Tokens;

namespace Markdown.Core.Translators
{
    public class MdToHtmlTranslator
    {
        public string TranslateTokensToHtml(IEnumerable<IToken> tokens)
        {
            var result = new StringBuilder();
            var headerTag = new HeaderTranslator().GetHeaderTag(tokens);
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