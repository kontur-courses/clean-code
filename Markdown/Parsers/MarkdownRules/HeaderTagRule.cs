using System;
using System.Collections.Generic;
using Markdown.IntermediateState;

namespace Markdown.Parsers.MarkdownRules
{
    class HeaderTagType : IParserRule
    {
        public string OpenTag { get; }
        public string CloseTag => "";
        public TagType TypeTag { get; }
        public ParserNode FindFirstElement(string source, HashSet<int> ignoredPositions, int startPosition = 0)
        {
            var openPosition = startPosition - 1;
            do
            {
                openPosition = source.IndexOf(OpenTag, openPosition + 1, StringComparison.Ordinal);
            } while (openPosition != -1 && openPosition != 0 && source[openPosition - 1] != '\n');
            if (openPosition == -1)
                return null;

            var closePosition = source.IndexOf('\n', openPosition);
            closePosition = closePosition == -1 ? source.Length : closePosition;
            return new ParserNode(TypeTag, openPosition, closePosition, this);
        }

        public bool CanUseInCurrent(TagType tagType)
        {
            return false;
        }

        public HeaderTagType(int headerType)
        {
            if (headerType > 0 && headerType < 7)
            {
                TypeTag = (TagType) headerType;
            }
            else throw new ArgumentException("Header type must be 1 to 6");

            OpenTag = new string('#', headerType) + " ";
        }
    }
}
