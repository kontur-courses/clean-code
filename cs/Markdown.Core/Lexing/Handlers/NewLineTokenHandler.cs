namespace Markdown.Core.Lexing.Handlers;

public class NewLineTokenHandler : ITokenHandler
{
    public bool CanHandle(ReadOnlySpan<char> source, int index) =>
        source[index] is '\n' or '\r';

    public int Handle(ReadOnlyMemory<char> source, int index, List<Token> tokens)
    {
        var span = source.Span;
        var length = source.Length;

        if (span[index] == '\r' && index + 1 < length && span[index + 1] == '\n')
        {
            tokens.Add(new Token(TokenKind.NewLine, source.Slice(index, 2)));
            return 2;
        }

        if (span[index] == '\n')
        {
            tokens.Add(new Token(TokenKind.NewLine, source.Slice(index, 1)));
            return 1;
        }
        
        tokens.Add(new Token(TokenKind.Text, source.Slice(index, 1)));
        return 1;
    }
}
