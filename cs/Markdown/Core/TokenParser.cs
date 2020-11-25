using Markdown.Core;
using Markdown.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class TokenParser
    {
        public bool IsTokenCorrupted { get; set; }
        public string PartBeforeTokenStart { get; set; }
        public string PartAfterTokenEnd { get; set; }

        protected Func<string, bool> nestedTokenValidator;
        protected int corruptedOffset;
        protected string formattingString;
        protected TokenType type;

        public virtual Token ParseToken(List<TokenPart> text, int position)
        {
            var tokenValue = new StringBuilder();
            if (IsTokenCorrupted)
            {
                tokenValue.Append(formattingString);
                return ParseToken(text, position, tokenValue, TokenType.Simple);
            }
            return ParseToken(text, position, tokenValue, type);
        }

        protected Token ParseToken(List<TokenPart> text, int position,
            StringBuilder tokenValue, TokenType type)
        {
            var parserOperator = new ParserOperator();
            if (text.Count == 0 && !IsTokenCorrupted)
                return CreateEmptyToken(tokenValue, position, parserOperator);
            CollectToken(text, tokenValue, parserOperator);
            var value = tokenValue.ToString();
            if (text.Count != 0 && CheckCorrectTokenValue(tokenValue,
                parserOperator,
                text.FirstOrDefault().Value,
                text.LastOrDefault().Value))
            {
                RecoverTokenValue(tokenValue, parserOperator);
                type = TokenType.Simple;
                value = tokenValue.ToString();
            }
            var nestedTokens = parserOperator.GetTokens();
            var token = new Token(position, value, type);
            token.SetNestedTokens(nestedTokens);
            return token;
        }

        protected virtual void CollectToken(List<TokenPart> text,
            StringBuilder tokenValue, ParserOperator parserOperator)
        {
            var isIntoToken = false;
            var offset = IsTokenCorrupted ? corruptedOffset : 0;
            foreach (var bigram in text.GetBigrams())
            {
                var part = bigram.Previous;
                if (nestedTokenValidator(part.Value) && !part.Escaped)
                {
                    if (isIntoToken)
                    {
                        parserOperator.Position = offset;
                        parserOperator.AddTokenPart(bigram);
                    }
                    isIntoToken = !isIntoToken;
                }
                else if (!isIntoToken)
                {
                    tokenValue.Append(part.Value);
                    offset += part.Value.Length;
                }
                if (isIntoToken)
                    parserOperator.AddTokenPart(bigram);
            }
            parserOperator.Position = offset;
        }

        protected virtual void RecoverTokenValue(StringBuilder value, ParserOperator parserOperator)
        {
            parserOperator.Position += corruptedOffset;
            value.Insert(0, formattingString);
            value.Append(formattingString);
        }

        private static bool CheckCorrectDeclaration(string start, string end)
        {
            if (start == null && end == null) return true;
            return ParserOperator.IsCorrectEnd(end) &&
                ParserOperator.IsCorrectStart(start);
        }

        private bool IsTokenInPartWord(string start, string value)
        {
            return PartBeforeTokenStart != null &&
                CheckCorrectDeclaration(start, PartBeforeTokenStart) &&
                value.Contains(" ");
        }

        private Token CreateEmptyToken(StringBuilder tokenValue, int position, ParserOperator parserOperator)
        {
            RecoverTokenValue(tokenValue, parserOperator);
            return new Token(position, tokenValue.ToString(), TokenType.Simple);
        }

        private bool CheckCorrectTokenValue(StringBuilder value, ParserOperator parserOperator,
            string tokenStart, string tokenEnd)
        {
            if (IsTokenCorrupted) return false;
            var tokenValue = value.ToString();
            var isInsideDigitText = tokenValue.IsDigit() && (PartAfterTokenEnd?.IsDigit() ?? false);
            var isInsideInDifferentPartWords = IsTokenInPartWord(tokenStart, tokenValue);
            var isHaveCorrectStartAndEnd = !CheckCorrectDeclaration(tokenStart, tokenEnd);
            var isHaveUnpairedChars = !parserOperator.IsClose()
                && !IsTokenCorrupted
                && (parserOperator.TokenContainsFormattingStrings(new[] { "__", "_" }) || tokenValue.Contains("_"));

            return isHaveCorrectStartAndEnd
                || isHaveUnpairedChars
                || isInsideDigitText
                || isInsideInDifferentPartWords;
        }
    }
}
