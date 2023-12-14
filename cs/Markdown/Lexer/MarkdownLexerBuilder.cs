using Markdown.Filter;
using Markdown.Tokens.Types;

namespace Markdown.Lexer;

public class MarkdownLexerBuilder
{
    private readonly MarkdownLexer markdownLexer;

    public MarkdownLexerBuilder(ITokenFilter filter, char escapeSymbol)
    {
        markdownLexer = new MarkdownLexer(filter, escapeSymbol);
    }
    
    public MarkdownLexerBuilder WithTokenType(string typeSymbol, ITokenType type)
    {
        markdownLexer.RegisterTokenType(typeSymbol, type);
        return this;
    }

    public MarkdownLexer Build() => markdownLexer;
}