#nullable enable
using System;

namespace Markdown
{
    public class HeadingTokenReader : ITokenReader
    {
        public Token? TryReadToken(TextParser parser, string text, int index)
        {
            if (text[index] != '#' || index != 0)
                return null;

            var value = text.Substring(index, text.Length);
            return new Token(index, value, TokenType.Heading);
        }
    }
}