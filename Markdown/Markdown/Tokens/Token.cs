namespace Markdown.Tokens;

public class Token
{
    public Token(string opening, string ending, TokenType type)
    {
        Opening = opening;
        Ending = ending;
        FirstPosition = -1;
        Length = 0;
        TokenType = type;
        NestedTokens = new List<Token>();
    }

    public TokenType TokenType { get; set; }
    
    public string Opening { get; protected set; }
    
    public string Ending { get; protected set; }
    
    public Token? Parent { get; set; }
    
    public List<Token> NestedTokens { get; set; }
    
    public int FirstPosition { get; set; }

    public int LastPosition => FirstPosition + Length - 1;
    
    public bool HasStartPosition => FirstPosition >= 0;
    
    public int Length  { get; set; }
    
    public bool HasEndPosition => Length >= 0;

    public virtual bool CanStartsHere(string text, int index)
    {
        return Opening.IsSubstringAt(text, index);
    }

    public virtual bool CanEndsHere(string text , int index)
    {
        return Ending.IsSubstringAt(text, index);
    }
}