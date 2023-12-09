using Markdown.Tokens.Types;

namespace Markdown.Lexer;

public class LexerBuilder
{
    private readonly Lexer lexer = new();

    public LexerBuilder WithTokenType(string typeSymbol, ITokenType type)
    {
        lexer.RegisterTokenType(typeSymbol, type);
        return this;
    }

    public Lexer Build() => lexer;
}