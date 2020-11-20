using System.Collections.Generic;

namespace Markdown.Parsers
{
    public class BoldParser : TokenParser
    {
        public BoldParser()
        {
            nestedTokenValidator = new HashSet<string>() { "__", "_", "\\" }.Contains;
            corruptedOffset = 2;
            formattingString = "__";
            type = TokenType.Bold;
        }
    }
}
