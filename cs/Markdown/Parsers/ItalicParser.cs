using System.Collections.Generic;

namespace Markdown.Parsers
{
    public class ItalicParser : TokenParser
    {
        public ItalicParser()
        {
            nestedTokenValidator = new HashSet<string> { "_", "\\" }.Contains;
            corruptedOffset = 1;
            formattingString = "_";
            type = TokenType.Italic;
        }
    }
}
