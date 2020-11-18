using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers
{
    public class ItalicParser : TokenParser
    {
        public ItalicParser()
        {
            nestedTokenValidator = new HashSet<string> { "_", "\\" }.Contains;
            corruptedOffset = 1;
        }

        public override Token ParseToken(List<string> text, int position)
        {
            var tokenValue = new StringBuilder();
            if (IsTokenCorrupted)
            {
                tokenValue.Append("_");
                return ParseToken(text, position, tokenValue, TokenType.Simple);
            }
            return ParseToken(text, position, tokenValue, TokenType.Italic);
        }

        protected override void RecoverTokenValue(StringBuilder value, ParserOperator parserOperator)
        {
            parserOperator.Position += corruptedOffset;
            value.Insert(0, "_");
            value.Append("_");
        }
    }
}
