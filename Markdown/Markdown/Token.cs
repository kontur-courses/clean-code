namespace Markdown;

public class Token
{
    public int StartIndex { get; set; }
    public int TokenLength { get; set; }
    public TokenType Type { get; set; }
    public int TokenMarkLength { get; set; }
    public TokenWrappers TokenWrappers { get; private set; }

    public Token(int startIndex, int tokenLength, TokenType type, int tokenMarkLength)
    {
        this.StartIndex = startIndex;
        this.TokenLength = tokenLength;
        this.Type = type;
        this.TokenMarkLength = tokenMarkLength;
    }
    
    public Token SetTokenWrappers(TokenWrappers tokenWrappers)
    {
        this.TokenWrappers = tokenWrappers;
        return this;
    }
}