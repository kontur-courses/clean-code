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
                    var reader = new StrongTokenReader();
                    intersectedToken = reader.TryReadToken(parser, text, i);

                    if (intersectedToken != null)
                    {
                        i = intersectedToken.EndPosition;
                    }
                }

                if ((text[i] == '\n' || i + 1 == text.Length) && !parser.IsEmphasizedEndTag(text, i))
                {
                    value = text[index..i];
                    break;
                }

                if (!parser.IsEmphasizedEndTag(text, i))
                    continue;

                if (intersectedToken != null && intersectedToken.EndPosition > i)
                    return new Token(index, text[index..intersectedToken.EndPosition], TokenType.PlainText);

                return new Token(index, text.Substring(index, i - index + 1), TokenType.Emphasized);
            }

            return new Token(index, value, TokenType.PlainText);
        }
    }
}