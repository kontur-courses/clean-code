namespace Markdown.Core.Lexing.Handlers;

public class UnderscoreTokenHandler : ITokenHandler
{
    public bool CanHandle(ReadOnlySpan<char> source, int index) =>
        source[index] == '_';

    public int Handle(ReadOnlyMemory<char> source, int index, List<Token> tokens)
    {
        var span = source.Span;
        if (index + 1 < source.Length && span[index + 1] == '_')
        {
            tokens.Add(new Token(TokenKind.DoubleUnderscore, source.Slice(index, 2)));
            return 2;
        }

        tokens.Add(new Token(TokenKind.Underscore, source.Slice(index, 1)));
        return 1;
    }
}
