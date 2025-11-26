namespace Markdown.Core.Lexing.Handlers;

public class SpaceTokenHandler : ITokenHandler
{
    public bool CanHandle(ReadOnlySpan<char> source, int index) =>
        source[index] is ' ' or '\t';

    public int Handle(ReadOnlyMemory<char> source, int index, List<Token> tokens)
    {
        tokens.Add(new Token(TokenKind.Space, source.Slice(index, 1)));
        return 1;
    }
}
