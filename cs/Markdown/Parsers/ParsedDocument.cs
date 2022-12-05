using System.Collections.Generic;

namespace Markdown.Parsers
{
    public class ParsedDocument
    {
        public readonly List<ParsedTextBlock> TextBlocks;

        public ParsedDocument()
        {
            TextBlocks = new List<ParsedTextBlock>();
        }
    }
}
