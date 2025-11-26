namespace Markdown.Core.Lexing.Handlers;

public class ParenthesisTokenHandler : ITokenHandler
{
    public bool CanHandle(ReadOnlySpan<char> source, int index) =>
        source[index] is '(' or ')';

    public int Handle(ReadOnlyMemory<char> source, int index, List<Token> tokens)
    {
        var kind = source.Span[index] == '('
            ? TokenKind.LeftParen
            : TokenKind.RightParen;
        tokens.Add(new Token(kind, source.Slice(index, 1)));
        return 1;
    }
}
