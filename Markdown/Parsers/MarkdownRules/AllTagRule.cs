using System;
using System.Collections.Generic;
using System.Text;
using Markdown.IntermediateState;

namespace Markdown.Parsers.MarkdownRules
{
    class AllTagRule :IParserRule
    {
        public string OpenTag => "";
        public string CloseTag => "";
        public TagType TypeTag => TagType.All;
        public ParserNode FindFirstElement(string source, int startPosition = 0)
        {
            return null;
        }

        public bool CanUseInCurrent(TagType tagType)
        {
            return true;
        }
    }
}
