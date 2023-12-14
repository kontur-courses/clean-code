using Markdown.Filter;
using Markdown.Tokens.Types;

namespace Markdown.Lexer;

public class MarkdownLexerBuilder
{
    private readonly MarkdownLexer markdownLexer;

    public MarkdownLexerBuilder(ITokenFilter filter, IEscapeSymbolFilter escapeSymbolFilter)
    {
        markdownLexer = new MarkdownLexer(filter, escapeSymbolFilter);
    }
    
    public MarkdownLexerBuilder WithTokenType(string typeSymbol, ITokenType type)
    {
        markdownLexer.RegisterTokenType(typeSymbol, type);
        return this;
    }

    public MarkdownLexer Build() => markdownLexer;
}