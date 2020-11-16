using System.Text;

#nullable enable
namespace Markdown
{
    public class PlainTextTokenReader : ITokenReader
    {
        public Token? TryReadToken(TextParser parser, string text, int index)
        {
            var value = new StringBuilder();

            for (var i = index; i < text.Length; ++i)
            {
                Token? token = null;

                if (parser.IsEmphasizedStartTag(text, i))
                {
                    var reader = new EmphasizedTokenReader();
                    token = reader.TryReadToken(parser, text, i);
                }
                else if (parser.IsStrongStartTag(text, i))
                {
                    var reader = new StrongTokenReader();
                    token = reader.TryReadToken(parser, text, i);
                }

                if (token != null)
                {
                    if (token.Type != TokenType.PlainText)
                        return value.ToString() != "" ? new Token(index, value.ToString(), TokenType.PlainText) : null;

                    i = token.EndPosition;
                    value.Append(token.Value);
                    continue;
                }

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