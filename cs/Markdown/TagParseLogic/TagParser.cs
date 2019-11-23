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
                .Select(tag => tag.MarkdownTagOpenDesignation)
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
            if (TryGetOpenTagToken(inputString, index, markdownTagInfo, out tagToken))
                return true;
            if (TryGetCloseTagToken(inputString, index, markdownTagInfo, out tagToken))
                return true;
            return false;
        }

        private bool TryGetOpenTagToken(string inputString, int index, MarkdownTagInfo markdownTagInfo, out TagToken tagToken)
        {
            tagToken = null;
            var markdownTagDesignation = markdownTagInfo.MarkdownTagOpenDesignation;
            
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

            if (markdownTagInfo.IsOpeningTag(markdownTagDesignation, previousTagSymbol, nextTagSymbol, specialSymbols))
            {
                tagToken = new TagToken(markdownTagInfo, index, TagTokenType.Opening);
                return true;
            }

            return false;
        }
        
        private bool TryGetCloseTagToken(string inputString, int index, MarkdownTagInfo markdownTagInfo, out TagToken tagToken)
        {
            tagToken = null;
            var markdownTagDesignation = markdownTagInfo.MarkdownTagCloseDesignation;
            
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

            if (markdownTagInfo.IsClosingTag(markdownTagDesignation, previousTagSymbol, nextTagSymbol, specialSymbols))
            {
                tagToken = new TagToken(markdownTagInfo, index, TagTokenType.Closing);
                return true;
            }

            return false;
        }
    }
}
