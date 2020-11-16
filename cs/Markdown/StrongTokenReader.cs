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
                    var reader = new EmphasizedTokenReader();
                    intersectedToken = reader.TryReadToken(parser, text, i);

                    if (intersectedToken != null)
                    {
                        i = intersectedToken.EndPosition;
                    }
                }

                if (text[i] == '\n' || i + 1 == text.Length && !parser.IsStrongStartTag(text, i))
                {
                    value = text[index..i];
                    break;
                }

                if (!parser.IsStrongEndTag(text, i))
                    continue;

                if (intersectedToken != null && intersectedToken.EndPosition > i)
                    return new Token(index, text[index..intersectedToken.EndPosition], TokenType.PlainText);

                return new Token(index, text.Substring(index, i - index + 2), TokenType.Strong);
            }

            return new Token(index, value, TokenType.PlainText);
        }
    }
}