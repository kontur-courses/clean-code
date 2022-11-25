namespace Markdown.Tokens;

public class Token
{
    public Token(string opening, string ending, TokenType type)
    {
        Opening = opening;
        Ending = ending;
        FirstPosition = -1;
        TokenType = type;
        NestedTokens = new List<Token>();
    }

    public TokenType TokenType { get; }

    public string Opening { get; protected init; }

    public string Ending { get; protected init; }

    public Token? Parent { get; set; }

    public List<Token> NestedTokens { get; }

    public int FirstPosition { get; set; }

    public int LastPosition => FirstPosition + Length - 1;

    public virtual int Length { get; set; }

    public virtual bool CanStartsHere(string text, int index)
    {
        return Opening.IsSubstringAt(text, index);
    }

    public virtual bool CanEndsHere(string text, int index)
    {
        return Ending.IsSubstringAt(text, index);
    }
}