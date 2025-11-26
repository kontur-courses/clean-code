namespace Markdown.Core.Lexing.Handlers;

public class HashTokenHandler : ITokenHandler
{
    public bool CanHandle(ReadOnlySpan<char> source, int index) =>
        source[index] == '#';

    public int Handle(ReadOnlyMemory<char> source, int index, List<Token> tokens)
    {
        var span = source.Span;
        if (IsAtLineStart(tokens) && index + 1 < source.Length && span[index + 1] == ' ')
            tokens.Add(new Token(TokenKind.Hash, source.Slice(index, 1)));
        else
            tokens.Add(new Token(TokenKind.Text, source.Slice(index, 1)));
        return 1;
    }

    private static bool IsAtLineStart(List<Token> tokens) =>
        tokens.Count == 0 || tokens[^1].Kind == TokenKind.NewLine;
}
