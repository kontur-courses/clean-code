using System.Collections.Generic;
using System.Linq;
using Markdown.Core.HTMLTags;
using Markdown.Core.Infrastructure;
using Markdown.Core.Tokens;

namespace Markdown.Core.Translators
{
    public class HeaderTranslator
    {
        public string GetHeaderTag(IEnumerable<IToken> tokens)
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
    }
}