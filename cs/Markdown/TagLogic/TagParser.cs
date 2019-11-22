using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class TagParser
    {
        private readonly HashSet<char> specialSymbols;
        private readonly MarkdownTag[] markdownTags;

        public TagParser(MarkdownTag[] markdownTags)
        {
            specialSymbols = markdownTags
                .Select(tag => tag.TagDesignation)
                .SelectMany(x => x)
                .ToHashSet();
            this.markdownTags = markdownTags;
        }

        public IEnumerable<TagToken> Parse(string inputString)
        {
            for (var i = 0; i < inputString.Length; i++)
                foreach (var markdownTag in markdownTags)
                    if (TryGetTagToken(inputString, i, markdownTag, out var tag))
                        yield return tag;
        }

        private bool TryGetTagToken(string inputString, int index, MarkdownTag markdownTag, out TagToken tagToken)
        {
            tagToken = null;
            var tagDesignation = markdownTag.TagDesignation;

            if (index + tagDesignation.Length > inputString.Length)
                return false;
            
            if (inputString.Substring(index, tagDesignation.Length) != tagDesignation)
                return false;
            
            var previousTagSymbol = (index != 0) 
                ? inputString[index - 1]
                : ' ';

            var nextTagSymbol = (index + tagDesignation.Length < inputString.Length)
                ? inputString[index + tagDesignation.Length]
                : ' ';

            if (markdownTag.IsOpeningTag(previousTagSymbol, nextTagSymbol, specialSymbols))
            {
                tagToken = new TagToken(markdownTag, index, TagTokenType.Opening);
                return true;
            }

            if (markdownTag.IsClosingTag(previousTagSymbol, nextTagSymbol, specialSymbols))
            {
                tagToken = new TagToken(markdownTag, index, TagTokenType.Closing);
                return true;
            }

            tagToken = null;
            return false;
        }
    }
}
