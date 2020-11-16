#nullable enable
using System;
using System.Text;

namespace Markdown
{
    public class HeadingTokenReader : ITokenReader
    {
        public Token? TryReadToken(TextParser parser, string text, int index)
        {
            if (text[index] != '#')
                return null;

            var value = new StringBuilder();
            for (var i = index; i < text.Length; ++i)
            {
                if (text[i] == '\n')
                    break;

                value.Append(text[i]);
            }

            return new Token(index, value.ToString(), TokenType.Heading);
        }
    }
}