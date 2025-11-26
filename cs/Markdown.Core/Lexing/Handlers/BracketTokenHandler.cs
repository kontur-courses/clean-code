namespace Markdown.Core.Lexing.Handlers;

public class BracketTokenHandler : ITokenHandler
{
    public bool CanHandle(ReadOnlySpan<char> source, int index) =>
        source[index] is '[' or ']';

    public int Handle(ReadOnlyMemory<char> source, int index, List<Token> tokens)
    {
        var kind = source.Span[index] == '['
            ? TokenKind.LeftBracket
            : TokenKind.RightBracket;
        tokens.Add(new Token(kind, source.Slice(index, 1)));
        return 1;
    }
}
