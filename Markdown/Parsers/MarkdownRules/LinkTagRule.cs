using System;
using System.Collections.Generic;
using System.Text;
using Markdown.IntermediateState;

namespace Markdown.Parsers.MarkdownRules
{
    class LinkTagRule : IParserRule
    {
        public string OpenTag => "[";
        public string CloseTag => "";
        public TagType TypeTag { get; }


        public ParserNode FindFirstElement(string source, HashSet<int> ignoredPositions, int startPosition = 0)
        {
            throw null;
        }

        public bool CanUseInCurrent(TagType tagType)
        {
            throw new NotImplementedException();
        }
    }
}
