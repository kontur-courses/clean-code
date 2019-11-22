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
        public ParserNode FindFirstElement(string source, HashSet<int> ignoredTags, int startPosition = 0)
        {
            var openPosition = startPosition - 1;
            do
            {
                openPosition = source.IndexOf("\n\n", openPosition + 1, StringComparison.Ordinal);
            } while (openPosition != -1 && ignoredTags.Contains(openPosition));
            while (openPosition == -1)
                return null;

            var closePosition = openPosition + 1;
            do
            {
                closePosition = source.IndexOf("\n\n", closePosition + 1, StringComparison.Ordinal);
            } while (closePosition != -1 && ignoredTags.Contains(closePosition));

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
