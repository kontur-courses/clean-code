#nullable enable
namespace Markdown
{
    public class StrongTokenReader : ITokenReader
    {
        public Token? TryReadToken(TextParser parser, string text, int index)
        {
            if (!parser.IsStrongStartTag(text, index))
                return null;

            var value = "";
            Token? intersectedToken = null;

            for (var i = index + 2; i < text.Length; i++)
            {
                if (parser.IsEmphasizedStartTag(text, i))
                {
                    intersectedToken = TryReadToken(parser, text, i);
                }

                if (!parser.IsStrongEndTag(text, i))
                    continue;

                if (intersectedToken != null && intersectedToken.Length > i)
                    break;

                value = text.Substring(index, i - index + 2);
                break;
            }

            return value != "" ? new Token(index, value, TokenType.Strong) : null;
        }
    }
}