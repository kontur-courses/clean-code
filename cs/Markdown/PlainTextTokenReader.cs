using System.Text;

namespace Markdown
{
    public class PlainTextTokenReader: ITokenReader
    {
        public Token? TryReadToken(string text, int index)
        {
            var value = new StringBuilder();
            value.Append(text[index]);

            for (var i = index + 1; i < text.Length; ++i)
            {
                if (IsNotPlainTextEnd(text, i))
                {
                    value.Append(text[i]);
                    continue;
                }

                break;
            }

            return new Token(index, value.ToString(), TokenType.PlainText);
        }

        private static bool IsNotPlainTextEnd(string text, int index)
        {
            return text[index] != '#'
                   && text[index] != '_';
        }
    }
}