using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers
{
    public class HeaderParser : TokenParser
    {
        public HeaderParser()
        {
            nestedTokenValidator = TokenReader.IsFormattingString;
            corruptedOffset = 1;
        }

        public override Token ParseToken(IEnumerable<string> text, int position)
        {
            var tokenValue = new StringBuilder();
            if (PartBeforeTokenStart != null && PartBeforeTokenStart != "\\\\")
            {
                tokenValue.Append("#");
                return ParseToken(text, position, tokenValue, TokenType.Simple);
            }
            return ParseToken(text, position, tokenValue, TokenType.Header);
        }

        protected override void CollectToken(IEnumerable<string> text, StringBuilder tokenValue, ParserOperator parserOperator)
        {
            var isIntoToken = false;
            var offset = 0;
            foreach (var part in text)
                if (nestedTokenValidator(part))
                {
                    if (!isIntoToken)
                        parserOperator.Position = offset;
                    parserOperator.AddTokenPart(part);
                    isIntoToken = !parserOperator.IsClose();
                }
                else if (!isIntoToken)
                {
                    tokenValue.Append(part);
                    offset += part.Length;
                }
                else
                    parserOperator.AddTokenPart(part);
        }

        protected override void RecoverTokenValue(StringBuilder value, ParserOperator parserOperator)
        {
            return;
        }
    }
}
