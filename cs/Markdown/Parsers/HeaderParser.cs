using Markdown.Extentions;
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

        public override Token ParseToken(List<string> text, int position)
        {
            var tokenValue = new StringBuilder();
            if (PartBeforeTokenStart != null && PartBeforeTokenStart != "\\\\")
            {
                tokenValue.Append("#");
                return ParseToken(text, position, tokenValue, TokenType.Simple);
            }
            return ParseToken(text, position, tokenValue, TokenType.Header);
        }

        protected override void CollectToken(List<string> text, StringBuilder tokenValue, ParserOperator parserOperator)
        {
            var isIntoToken = false;
            var offset = 0;
            foreach (var bigram in text.GetBigrams())
            {
                var part = bigram.Item1;
                if (nestedTokenValidator(part))
                {
                    if (!isIntoToken)
                        parserOperator.Position = offset;
                    parserOperator.AddTokenPart(bigram);
                    isIntoToken = !parserOperator.IsClose();
                }
                else if (!isIntoToken)
                {
                    tokenValue.Append(part);
                    offset += part.Length;
                }
                else
                    parserOperator.AddTokenPart(bigram);
            }
        }

        protected override void RecoverTokenValue(StringBuilder value, ParserOperator parserOperator)
        {
            return;
        }
    }
}
