namespace Markdown.Tokens;

public sealed class EscapeToken : Token
{
    public EscapeToken() : base(@"\", string.Empty, TokenType.Escape)
    {
        Length = Opening.Length;
    }

    public override bool CanEndsHere(string text, int index)
    {
        var next = index + Opening.Length - 1;
        return base.CanEndsHere(text, index)
               && next < text.Length
               && text[next] != '\n'
               && TokenSelector.SelectLongestSuitableToken(text, next) is not null;
    }
}