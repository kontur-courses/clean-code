using System;
using System.Collections.Generic;
using System.Text;
using Markdown.IntermediateState;
using Markdown.Parsers.MarkdownRules;

namespace Markdown.Parsers
{
    public interface IParserRule
    {
        string OpenTag { get; }
        string CloseTag { get; }
        TagType TypeTag { get; }
        ParserNode FindFirstElement(string source, HashSet<int> ignoredPositions, int startPosition=0);
        bool CanUseInCurrent(TagType tagType);
    }
}
