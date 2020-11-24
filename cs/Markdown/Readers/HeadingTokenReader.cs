using System.Linq;
using Markdown.Tokens;

namespace Markdown.Readers
{
    public class HeadingTokenReader : ITokenReader
    {
        public bool TryReadToken(string text, string context, int index, out Token? token)
        {
            token = null;

            if (!IsHeadingStart(text, index))
                return false;

            var value = new string(text[index..]
                .TakeWhile(x => x != '\n' && x != '\r')
                .ToArray());

            token = new HeadingToken(index, value[2..], index + value.Length - 1);
            return true;
        }

        private static bool IsHeadingStart(string text, int index)
        {
            return text[index] == '#'
                   && index + 1 != text.Length
                   && text[index + 1] == ' '
                   && (index == 0 || index - 1 < 0 || text[index - 1] == '\n' || text[index - 1] == '\r');
        }
    }
}