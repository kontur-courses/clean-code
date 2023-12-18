namespace Markdown.Tokens.Decorators;

public class TokenFilteringDecorator : Token
{
    public bool IsMarkedForDeletion { get; set; }
    
    public TokenFilteringDecorator(Token token) : base(token.Type, token.IsClosingTag, token.StartingIndex, token.Length)
    {
    }
}