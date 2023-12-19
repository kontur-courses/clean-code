using Markdown.Filter;
using Markdown.Tokens.Types;

namespace Markdown.Lexer;

public class MarkdownLexerBuilder
{
    private readonly ITokenFilter filter;
    private readonly char escapeSymbol;
    private readonly HashSet<ITokenType> tokenTypes = new(new TokenTypeEqualityComparer());

    public MarkdownLexerBuilder(ITokenFilter filter, char escapeSymbol = '\\')
    {
        this.filter = filter;
        this.escapeSymbol = escapeSymbol;
    }

    public MarkdownLexerBuilder WithTokenType(ITokenType type)
    {
        ValidateTokenType(type);
        tokenTypes.Add(type);
        return this;
    }

    public MarkdownLexer Build() => new(filter, tokenTypes, escapeSymbol);

    private static void ValidateTokenType(ITokenType type)
    {
        if (type is null)
            throw new ArgumentException("Token type cannot be null.");
        if (type.Value is null)
            throw new ArgumentException("Type value cannot be null.");
        if (type.Representation(true) is null || type.Representation(false) is null)
            throw new ArgumentException("Token type representation cannot be null.");
    }
}