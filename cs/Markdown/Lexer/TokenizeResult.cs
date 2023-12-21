using Markdown.Tokens;

namespace Markdown.Lexer;

public class TokenizeResult
{
    public List<Token> Tokens { get; }

    public IReadOnlyDictionary<int, bool> EscapeSymbolsPos { get; }

    public TokenizeResult(List<Token> tokens, IReadOnlyDictionary<int, bool> escapeSymbolsPos)
    {
        Tokens = tokens;
        EscapeSymbolsPos = escapeSymbolsPos;
    }
}