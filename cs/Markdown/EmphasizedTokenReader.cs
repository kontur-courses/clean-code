#nullable enable
namespace Markdown
{
    public class EmphasizedTokenReader : ITokenReader
    {
        public Token? TryReadToken(TextParser parser, string text, int index)
        {
            if (!parser.IsEmphasizedStartTag(text, index))
                return null;

            var value = "";
            Token? intersectedToken = null;

            for (var i = index + 1; i < text.Length; i++)
            {
                if (parser.IsStrongStartTag(text, i))
                {
                    intersectedToken = TryReadToken(parser, text, i);
                }

                if (!parser.IsEmphasizedEndTag(text, i))
                    continue;

                if (intersectedToken != null && intersectedToken.Length > i)
                    break;

                value = text.Substring(index, i - index + 1);
                break;
            }

            return value != "" ? new Token(index, value, TokenType.Emphasized) : null;
        }
    }
}