using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Markdown
{
    public class TokenParser
    {
        public bool TokenCorruptedMode { get; set; }
        public string PartBeforeTokenStart { get; set; }

        protected Func<string, bool> nestedTokenValidator;
        protected int corruptedOffset;

        public virtual Token ParseToken(IEnumerable<string> text, int position)
        {
            return new Token(0, "", TokenType.Simple);
        }

        protected Token ParseToken(IEnumerable<string> text, int position,
            StringBuilder tokenValue, TokenType type)
        {
            var parserOperator = new ParserOperator();
            if (text.Count() == 0)
                return CreateEmptyToken(tokenValue, position, parserOperator);
            CollectToken(text, tokenValue, parserOperator);

            if (!parserOperator.IsClose() && !TokenCorruptedMode || !CheckCorrectToken(text))
            {
                RecoverTokenValue(tokenValue, parserOperator);
                type = TokenType.Simple;
            }

            var nestedTokens = parserOperator.GetTokens();
            var value = tokenValue.ToString();

            if (value.IsDigit() || IsTokenInPartWord(text.First(), value))
            {
                RecoverTokenValue(tokenValue, parserOperator);
                type = TokenType.Simple;
                value = tokenValue.ToString();
            }

            var token = new Token(position, value, type);
            token.SetNestedTokens(nestedTokens);
            return token;
        }

        protected virtual void CollectToken(IEnumerable<string> text,
            StringBuilder tokenValue, ParserOperator parserOperator)
        {
            var isIntoToken = false;
            var offset = TokenCorruptedMode ? corruptedOffset : 0;
            foreach (var part in text)
            {
                if (nestedTokenValidator(part))
                {
                    if (part == "\\")
                        parserOperator.Position = offset;
                    if (isIntoToken)
                    {
                        parserOperator.Position = offset;
                        parserOperator.AddTokenPart(part);
                    }
                    isIntoToken = !isIntoToken;
                }
                else if (!isIntoToken)
                {
                    tokenValue.Append(part);
                    offset += part.Length;
                }
                if (isIntoToken)
                    parserOperator.AddTokenPart(part);
            }
            parserOperator.Position = offset;
        }

        protected virtual void RecoverTokenValue(StringBuilder value, ParserOperator parserOperator)
        {
            parserOperator.Position += corruptedOffset;
            value.Insert(0, "__");
            value.Append("__");
        }

        private bool CheckCorrectToken(IEnumerable<string> text)
        {
            if (text.Count() == 0) return true;
            return ParserOperator.IsCorrectEnd(text.LastOrDefault()) &&
                ParserOperator.IsCorrectStart(text.FirstOrDefault());
        }

        private bool IsTokenInPartWord(string start, string value)
        {
            return PartBeforeTokenStart != null &&
                CheckCorrectToken(new[] { PartBeforeTokenStart, start }) &&
                value.Contains(" ");
        }

        private Token CreateEmptyToken(StringBuilder tokenValue, int position, ParserOperator parserOperator)
        {
            RecoverTokenValue(tokenValue, parserOperator);
            return new Token(position, tokenValue.ToString(), TokenType.Simple);
        }
    }
}
