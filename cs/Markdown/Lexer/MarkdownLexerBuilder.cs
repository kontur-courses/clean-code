using Markdown.Tokens.Types;
using Markdown.Validator;

namespace Markdown.Lexer;

public class MarkdownLexerBuilder
{
    private readonly MarkdownLexer markdownLexer;

    public MarkdownLexerBuilder(ITokenValidator validator)
    {
        markdownLexer = new MarkdownLexer(validator);
    }
    
    public MarkdownLexerBuilder WithTokenType(string typeSymbol, ITokenType type)
    {
        markdownLexer.RegisterTokenType(typeSymbol, type);
        return this;
    }

    public MarkdownLexer Build() => markdownLexer;
}