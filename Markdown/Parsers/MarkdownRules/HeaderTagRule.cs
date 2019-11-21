using System;
using Markdown.IntermediateState;

namespace Markdown.Parsers.MarkdownRules
{
    class HeaderTagType : IParserRule
    {
        public string OpenTag { get; }
        public string CloseTag => "";
        public TagType TypeTag { get; }
        public ParserNode FindFirstElement(string source, int startPosition = 0)
        {
            var openPosition = source.IndexOf(OpenTag, startPosition);
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
