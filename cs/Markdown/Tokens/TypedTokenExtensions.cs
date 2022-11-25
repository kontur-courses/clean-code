using System.Collections.Generic;

namespace Markdown.Tokens
{
    public static class TypedTokenExtensions
    {
        public static void AddTextBetween(this List<TypedToken> tokens, TypedToken previous, TypedToken next)
        {
            if (next.Start == previous.End + 1)
                return;

            var start = previous.End + 1;

            var length = next.Start - previous.End - 1;

            tokens.Add(new TypedToken(start, length, TokenType.Text));
        }

        public static void AddTextFromBeginningUpToTag(this List<TypedToken> tokens, TypedToken tagToken)
        {
            if (tagToken.Start == 0)
                return;

            tokens.AddTextBetween(new TypedToken(0, 0, TokenType.Text), tagToken);
        }

        public static void AddTextAfterTag(this List<TypedToken> tokens, TypedToken tagToken, int textLength)
        {
            if (textLength <= 0)
                return;

            tokens.AddTextBetween(tagToken, new TypedToken(tagToken.End + textLength + 1, 1, TokenType.Text));
        }
    }
}