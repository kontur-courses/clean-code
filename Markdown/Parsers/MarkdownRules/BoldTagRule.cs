using System;
using System.Linq;
using Markdown.IntermediateState;
using static System.Char;

namespace Markdown.Parsers.MarkdownRules
{
    class BoldTagRule : IParserRule
    {
        public string OpenTag => "__";
        public string CloseTag => "__";
        public TagType TypeTag => TagType.Bold;
        public ParserNode FindFirstElement(string source, int startPosition=0)
        {
            var openPosition = startPosition - 1;
            do
            {
                openPosition = source.IndexOf(OpenTag, openPosition + 1, StringComparison.Ordinal);
            } while (openPosition != -1 && !IsCorrectTag(source, openPosition, true));

            if (openPosition == -1)
                return null;

            var closePosition = openPosition + 1;
            do
            {
                closePosition = source.IndexOf(CloseTag, closePosition + 1, StringComparison.Ordinal);
            } while (closePosition != -1 && (!IsCorrectTag(source, closePosition, false) ||
                     IsUnderlineString(source, openPosition, closePosition)));

            return closePosition == -1 ? null : new ParserNode(TypeTag, openPosition, closePosition + 2, this);
        }

        public bool CanUseInCurrent(TagType tagType)
        {
            return tagType != TagType.H1 && tagType != TagType.H2 && tagType != TagType.H3 && tagType != TagType.H4 &&
                   tagType != TagType.H5 && tagType != TagType.H6;
        }

        private static bool IsCorrectTag(string source, int position, bool isOpenTag)
        {
            if (isOpenTag)
            {
                return (position == 0 || !IsDigit(source[position - 1])) && position + 2 < source.Length &&
                       !IsDigit(source[position + 2]) && !IsWhiteSpace(source[position + 2]);
            }

            return position > 0 && !IsDigit(source[position - 1]) && !IsWhiteSpace(source[position - 1]) &&
                   (position + 2 >= source.Length || !IsDigit(source[position + 2]) && source[position + 2] != '_');
        }

        public static bool IsUnderlineString(string source, int startPosition, int endPosition)
        {
            return source.Skip(startPosition).Take(endPosition - startPosition).All(c => c == '_');
        }
    }
}
