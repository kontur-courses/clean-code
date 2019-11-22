using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Markdown.IntermediateState;

namespace Markdown.Parsers.MarkdownRules
{
    class RawTagRule : IParserRule
    {
        public string OpenTag => "```\n";
        public string CloseTag => "\n```";
        public TagType TypeTag => TagType.Raw;
        public ParserNode FindFirstElement(string source, HashSet<int> ignoredPositions, int startPosition = 0)
        {
            var openPosition = startPosition - 1;

            do
            {
                openPosition = source.IndexOf(OpenTag, openPosition + 1, StringComparison.Ordinal);
            } while (openPosition != -1 && ignoredPositions.Contains(openPosition));

            if (openPosition == -1)
                return null;

            var closePosition = openPosition + 3;

            do
            {
                closePosition = source.IndexOf(CloseTag, closePosition + 1, StringComparison.Ordinal);
            } while (closePosition != -1 && ignoredPositions.Contains(closePosition));

            return closePosition == -1 ? null : new ParserNode(TypeTag, openPosition, closePosition + 4, this);
        }

        public bool CanUseInCurrent(TagType tagType)
        {
            return false;
        }
    }
}
