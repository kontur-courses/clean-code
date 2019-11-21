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
        public ParserNode FindFirstElement(string source, int startPosition = 0)
        {
            var openPosition = source.IndexOf(OpenTag, startPosition, StringComparison.Ordinal);
            if (openPosition == -1)
                return null;

            var closePosition = source.IndexOf(CloseTag, openPosition + 4, StringComparison.Ordinal);
            closePosition = closePosition == -1 ? source.Length : closePosition;

            return closePosition == -1 ? null : new ParserNode(TypeTag, openPosition, closePosition + 4, this);
        }

        public bool CanUseInCurrent(TagType tagType)
        {
            return false;
        }
    }
}
