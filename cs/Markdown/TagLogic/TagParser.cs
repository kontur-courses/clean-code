using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    internal class TagParser
    {
        private readonly HashSet<char> specialSymbols;
        private readonly List<string> tagDesignations;

        public TagParser(string[] tagDesignations)
        {
            specialSymbols = tagDesignations.SelectMany(x => x).ToHashSet();
            this.tagDesignations = tagDesignations.ToList();
        }

        public IEnumerable<Tag> Parse(string inputString)
        {
            for (var i = 0; i < inputString.Length; i++)
                foreach (var tagDesignation in tagDesignations)
                    if (TryGetTag(inputString, i, tagDesignation, out var tag))
                        yield return tag;
        }

        private bool TryGetTag(string inputString, int tagIndex, string tagDesignation, out Tag tag)
        {
            tag = null;

            if (tagIndex + tagDesignation.Length > inputString.Length)
                return false;
            
            if (inputString.Substring(tagIndex, tagDesignation.Length) != tagDesignation)
                return false;
            
            var previousTagSymbol = (tagIndex != 0) 
                ? inputString[tagIndex - 1]
                : ' ';

            var nextTagSymbol = (tagIndex + tagDesignation.Length < inputString.Length)
                ? inputString[tagIndex + tagDesignation.Length]
                : ' ';

            if (IsOpeningTag(previousTagSymbol, nextTagSymbol))
            {
                tag = new Tag(tagDesignation, tagIndex, 
                    TagType.Opening, MarkdownTransformerToHtml.TagsInfo[tagDesignation].Priority);
                return true;
            }

            if (IsClosingTag(previousTagSymbol, nextTagSymbol))
            {
                tag = new Tag(tagDesignation, tagIndex,
                    TagType.Closing, MarkdownTransformerToHtml.TagsInfo[tagDesignation].Priority);
                return true;
            }

            tag = null;
            return false;
        }

        private bool IsOpeningTag(char previousSeparatorSymbol, char nextSeparatorSymbol) =>
            char.IsWhiteSpace(previousSeparatorSymbol)
            && !char.IsWhiteSpace(nextSeparatorSymbol)
            && !specialSymbols.Contains(previousSeparatorSymbol) 
            && !specialSymbols.Contains(nextSeparatorSymbol);

        private bool IsClosingTag(char previousSeparatorSymbol, char nextSeparatorSymbol) =>
            char.IsWhiteSpace(nextSeparatorSymbol)
            && !char.IsWhiteSpace(previousSeparatorSymbol)
            && !specialSymbols.Contains(nextSeparatorSymbol)
            && !specialSymbols.Contains(previousSeparatorSymbol);
    }
}
