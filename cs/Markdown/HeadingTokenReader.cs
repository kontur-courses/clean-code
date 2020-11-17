using System.Linq;

namespace Markdown
{
    public class HeadingTokenReader : ITokenReader
    {
        public Token? TryReadToken(string text, int index)
        {
            if (text[index] != '#')
                return null;

            var value = new string(text
                .Skip(index)
                .TakeWhile(x => x != '\n')
                .ToArray()
            );

            return new Token(index, value, TokenType.Heading);
        }
    }
}