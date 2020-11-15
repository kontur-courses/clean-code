using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers
{
    public class BoldParser : TokenParser
    {
        public BoldParser()
        {
            nestedTokenValidator = TokenReader.IsFormattingString;
            corruptedOffset = 2;
        }

        public override Token ParseToken(IEnumerable<string> text, int position)
        {
            var tokenValue = new StringBuilder();
            if (TokenCorruptedMode)
            {
                tokenValue.Append("__");
                return ParseToken(text, position, tokenValue, TokenType.Simple);
            }
            return ParseToken(text, position, tokenValue, TokenType.Bold);
        }

        protected override void RecoverTokenValue(StringBuilder value, ParserOperator parserOperator)
        {
            parserOperator.Position += corruptedOffset;
            value.Insert(0, "__");
            value.Append("__");
        }
    }
}
