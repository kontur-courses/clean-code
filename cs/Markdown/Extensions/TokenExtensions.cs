using Markdown.Primitives;

namespace Markdown.Extensions;

public static class TokenExtensions
{
    public static TagNode ToTagNode(this Token token) => token.ToTag().ToTagNode();
        
    public static Tag ToTag(this Token token) => token.Type switch
    {
        TokenType.Bold => Tags.Bold(Tokens.Bold.Value),
        TokenType.Italic => Tags.Italic(Tokens.Italic.Value),
        TokenType.Header1 => Tags.Header1(Tokens.Header1.Value),
        _ => Tags.Text(token.Value)
    };
        
    public static Token ToTextToken(this Token token) => Tokens.Text(token.Value);
}