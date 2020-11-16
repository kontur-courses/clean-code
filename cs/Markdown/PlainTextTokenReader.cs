using System.Text;

#nullable enable
namespace Markdown
{
    public class PlainTextTokenReader : ITokenReader
    {
        public Token? TryReadToken(TextParser parser, string text, int index)
        {
            var value = new StringBuilder();
            value.Append(text[index]);

            for (var i = index + 1; i < text.Length; ++i)
            {
                if (parser.IsPlainText(text, i))
                {
                    value.Append(text[i]);
                    continue;
                }

                break;
            }

            return new Token(index, value.ToString(), TokenType.PlainText);
        }
    }
}