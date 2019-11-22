using System;
using System.Collections.Generic;
using Markdown.IntermediateState;

namespace Markdown.Parsers.MarkdownRules
{
    class ItalicTagRule : IParserRule
    {
        public string OpenTag => "_";
        public string CloseTag => "_";
        public TagType TypeTag => TagType.Italic;
        public ParserNode FindFirstElement(string source, HashSet<int> ignoredPositions, int startPosition = 0)
        {
            int openPosition = startPosition - 1;
            do
            {
                openPosition = source.IndexOf(OpenTag, openPosition + 1, StringComparison.Ordinal);
            } while (openPosition != -1 && (!IsOpenTag(source, openPosition) || ignoredPositions.Contains(openPosition)));

            if (openPosition == -1)
                return null;

            var closePosition = openPosition;
            do
            {
                closePosition = source.IndexOf(CloseTag, closePosition + 1, StringComparison.Ordinal);
            } while (closePosition != -1 && (!IsCloseTag(source, closePosition) || closePosition - openPosition < 2 ||
                                             ignoredPositions.Contains(openPosition) ||
                                             BoldTagRule.IsUnderlineString(source, openPosition, closePosition + 1)));

            return closePosition == -1
                ? null
                : new ParserNode(TypeTag, openPosition, closePosition + 1, this);
        }

        public bool CanUseInCurrent(TagType tagType)
        {
            return ((int) tagType < 1 || (int) tagType > 6) && tagType != TagType.Bold;
        }

        public bool IsOpenTag(string source, int position)
        {
            return (position == 0 || !char.IsDigit(source[position - 1])) && position < source.Length - 1 &&
                   !char.IsDigit(source[position + 1]) && !char.IsWhiteSpace(source[position + 1]);
        }

        public bool IsCloseTag(string source, int position)
        {
            return position > 0 && !char.IsDigit(source[position - 1]) && !char.IsWhiteSpace(source[position - 1]) &&
                   (position + 1 == source.Length || !char.IsDigit(source[position + 1]));
        }
    }
}
