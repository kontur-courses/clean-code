using System.Collections.Generic;
using System.Linq;
using Markdown.MarkdownTags;

namespace Markdown
{
    internal class TagParser
    {
        private readonly HashSet<char> specialSymbols;
        private readonly MarkdownTagInfo[] markdownTagsInfo;

        public TagParser(MarkdownTagInfo[] markdownTagsInfo)
        {
            specialSymbols = markdownTagsInfo
                .Select(tag => tag.MarkdownTagDesignation)
                .SelectMany(x => x)
                .ToHashSet();
            this.markdownTagsInfo = markdownTagsInfo;
        }

        public IEnumerable<TagToken> Parse(string inputString)
        {
            for (var i = 0; i < inputString.Length; i++)
                foreach (var markdownTag in markdownTagsInfo)
                    if (TryGetTagToken(inputString, i, markdownTag, out var tag))
                        yield return tag;
        }

        private bool TryGetTagToken(string inputString, int index, MarkdownTagInfo markdownTagInfo, out TagToken tagToken)
        {
            tagToken = null;
            var markdownTagDesignation = markdownTagInfo.MarkdownTagDesignation;

            if (index + markdownTagDesignation.Length > inputString.Length)
                return false;
            
            if (inputString.Substring(index, markdownTagDesignation.Length) != markdownTagDesignation)
                return false;
            
            var previousTagSymbol = (index != 0) 
                ? inputString[index - 1]
                : ' ';

            var nextTagSymbol = (index + markdownTagDesignation.Length < inputString.Length)
                ? inputString[index + markdownTagDesignation.Length]
                : ' ';

            if (markdownTagInfo.IsOpeningTag(previousTagSymbol, nextTagSymbol, specialSymbols))
            {
                tagToken = new TagToken(markdownTagInfo, index, TagTokenType.Opening);
                return true;
            }

            if (markdownTagInfo.IsClosingTag(previousTagSymbol, nextTagSymbol, specialSymbols))
            {
                tagToken = new TagToken(markdownTagInfo, index, TagTokenType.Closing);
                return true;
            }

            tagToken = null;
            return false;
        }
    }
}
