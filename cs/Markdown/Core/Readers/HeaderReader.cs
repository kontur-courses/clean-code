using System.Collections.Generic;
using Markdown.Core.HTMLTags;
using Markdown.Core.Infrastructure;
using Markdown.Core.Tokens;

namespace Markdown.Core.Readers
{
    public class HeaderReader
    {
        public Token ReadHeaderToken(string word, int wordPosition, List<int> escapedPositions)
        {
            foreach (var headerTag in TagsUtils.MdBeginningTags)
            {
                if (ReaderUtils.IsValidPositionForOpeningTag(word, headerTag, wordPosition, escapedPositions))
                    return new HTMLTagToken(wordPosition, headerTag, HTMLTagType.Header);
            }

            return null;
        }
    }
}