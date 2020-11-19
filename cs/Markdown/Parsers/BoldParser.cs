namespace Markdown.Parsers
{
    public class BoldParser : TokenParser
    {
        public BoldParser()
        {
            nestedTokenValidator = TokenReader.IsFormattingString;
            corruptedOffset = 2;
            formattingString = "__";
            type = TokenType.Bold;
        }
    }
}
