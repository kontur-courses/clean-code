using Markdown.Core;
using Markdown.Extentions;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Parsers
{
    public class HeaderParser : TokenParser
    {
        private int offset;
        public HeaderParser()
        {
            nestedTokenValidator = new HashSet<string>() { "__", "_", "\\" }.Contains;
            corruptedOffset = 1;
            formattingString = "#";
            type = TokenType.Header;
        }

        public override Token ParseToken(List<TokenPart> text, int position)
        {
            var tokenValue = new StringBuilder();
            if (PartBeforeTokenStart != null && PartBeforeTokenStart != "\\\\" || position != 0)
            {
                tokenValue.Append("#");
                offset = 1;
                return ParseToken(text, position, tokenValue, TokenType.Simple);
            }
            return ParseToken(text, position, tokenValue, TokenType.Header);
        }

        protected override void CollectToken(List<TokenPart> text, StringBuilder tokenValue, ParserOperator parserOperator)
        {
            var isIntoToken = false;
            foreach (var bigram in text.GetBigrams())
            {
                var part = bigram.Previous;
                if (nestedTokenValidator(part.Value) && !part.Escaped)
                {
                    if (!isIntoToken)
                        parserOperator.Position = offset;
                    parserOperator.AddTokenPart(bigram);
                    isIntoToken = !parserOperator.IsClose();
                }
                else if (!isIntoToken)
                {
                    tokenValue.Append(part.Value);
                    offset += part.Value.Length;
                }
                else
                    parserOperator.AddTokenPart(bigram);
            }
        }

        protected override void RecoverTokenValue(StringBuilder value, ParserOperator parserOperator)
        {
            parserOperator.Position += corruptedOffset;
            value.Insert(0, formattingString);
        }
    }
}
