using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.Tags;

namespace Markdown.Tokens
{
    public static class TokenExtensions
    {
        public static void AddTextBetween(this List<Token> tokens, Token previous, Token next)
        {
            if (next.Start == previous.End + 1)
                return;

            var start = previous.End + 1;

            var length = next.Start - previous.End - 1;

            tokens.Add(new Token(TokenType.Text, start, length));
        }

        public static void AddTextFromBeginningUpToTag(this List<Token> tokens, TagToken tagToken)
        {
            if (tagToken.Start == 0)
                return;

            tokens.AddTextBetween(new Token(TokenType.Text, 0, 0), tagToken);
        }

        public static void AddTextAfterTag(this List<Token> tokens, TagToken tagToken, int textLength)
        {
            if (textLength <= 0)
                return;

            tokens.AddTextBetween(tagToken, new Token(TokenType.Text, tagToken.End + textLength + 1, 1));
        }


    }
}
