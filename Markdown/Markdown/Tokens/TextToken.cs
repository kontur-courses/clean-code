using System.Text;

namespace Markdown.Tokens;

public class TextToken : Token
{
    public TextToken() : base(string.Empty, string.Empty, TokenType.Text)
    {
        Text = new StringBuilder();
    }

    public StringBuilder Text { get; }

    public override int Length => Text.Length;

    public static (TextToken Opening, TextToken Ending) ToText(Token token)
    {
        var opening = new TextToken { FirstPosition = token.FirstPosition, Length = token.Opening.Length };
        opening.Text.Append(token.Opening);
        var ending = new TextToken
        {
            FirstPosition = token.LastPosition - token.Opening.Length + (token.Opening.Length == 0 ? 0 : 1),
            Length = token.Ending.Length
        };
        ending.Text.Append(token.Ending);
        return (opening, ending);
    }
}