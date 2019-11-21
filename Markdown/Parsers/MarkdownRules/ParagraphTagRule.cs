using System;
using System.Collections.Generic;
using System.Text;
using Markdown.IntermediateState;
using NUnit.Framework.Constraints;

namespace Markdown.Parsers.MarkdownRules
{
    class ParagraphTagRule : IParserRule
    {
        public string OpenTag => "";
        public string CloseTag { get; }
        public TagType TypeTag => TagType.Paragraph;
        public ParserNode FindFirstElement(string source, int startPosition = 0)
        {
            var openPosition = source.IndexOf("\n\n", startPosition, StringComparison.Ordinal);
            if (openPosition == -1)
                return null;

            var closePosition = source.IndexOf("\n\n", openPosition + 2, StringComparison.Ordinal);
            closePosition = closePosition == -1 ? source.Length : closePosition;

            return closePosition - openPosition <= 2
                ? null
                : new ParserNode(TypeTag, openPosition, closePosition, this);
        }

        public bool CanUseInCurrent(TagType tagType)
        {
            return true;
        }
    }
}
