using Markdown.Tokens;
using Markdown.Tokens.Types;

namespace Markdown.Lexer;

public class Lexer : ILexer
{
    private readonly Dictionary<string, ITokenType> registeredTokenTypes = new();

    public void RegisterTokenType(string typeSymbol, ITokenType type)
    {
        registeredTokenTypes.Add(typeSymbol, type);
    }

    public IEnumerable<Token> Tokenize(string line)
    {
        throw new NotImplementedException();
    }
}