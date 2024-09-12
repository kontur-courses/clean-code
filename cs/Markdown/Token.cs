namespace Markdown;

public class Token
{
    public SyntaxKind Kind { get; private set; }
    public int Start { get; private set; }
    public string Text { get; private set; }

    public Token(SyntaxKind kind, int start, string text)
    {
        Kind = kind;
        Start = start;
        Text = text;
    }
}