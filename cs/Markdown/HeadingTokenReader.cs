using System.Linq;

namespace Markdown
{
    public class HeadingTokenReader : ITokenReader
    {
        public bool TryReadToken(string text, string context, int index, out Token? token)
        {
            token = null;

            if (text[index] != '#')
                return false;

            var value = new string(text
                .Skip(index)
                .TakeWhile(x => x != '\n')
                .ToArray()
            );

            token = new Token(index, value[1..], index + value.Length - 1, TokenType.Heading);
            return true;
        }
    }
}