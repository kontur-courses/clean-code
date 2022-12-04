using System.Collections.Generic;

namespace Markdown.Parsers
{
    public class ParsedDocument
    {
        //TODO: https://www.markdownguide.org/basic-syntax/
        public readonly List<ParsedTextBlock> TextBlocks;

        public ParsedDocument()
        {
            TextBlocks = new List<ParsedTextBlock>();
        }
    }
}
