using System;
using System.Collections.Generic;
using System.Text;
using Markdown.IntermediateState;

namespace Markdown.Parsers.MarkdownRules
{
    class RawTagRule : IParserRule
    {
        public string OpenTag { get; }
        public string CloseTag { get; }
        public TagType TypeTag { get; }
        public ParserNode FindFirstElement(string source, int startPosition = 0)
        {
            return null;
        }

        public bool CanUseInCurrent(TagType tagType)
        {
            throw new NotImplementedException();
        }
    }
}
