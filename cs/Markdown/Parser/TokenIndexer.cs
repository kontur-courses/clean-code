using Markdown;

public class TokenIndexer(List<Token> tokens)
{
    public readonly List<Token> Tokens = tokens;

    public int Index { get; set;}

    public bool End => Index >= Tokens.Count || Tokens[Index].Type == TokenType.EndOfFile;

    public Token GetTokenAt(int index)
    {
        if (index < 0 || index >= Tokens.Count)
            return new Token(TokenType.Whitespace, string.Empty);

        return Tokens[index];
    }

    private Token Peek()
    {
        return End ? Tokens[^1] : Tokens[Index];
    }
    
    public Token Consume()
    {
        return Tokens[Index++];
    }

    public bool Is(TokenType type)
    {
        return !End && Peek().Type == type;
    }
}
