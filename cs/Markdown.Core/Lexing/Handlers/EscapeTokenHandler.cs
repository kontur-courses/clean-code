namespace Markdown.Core.Lexing.Handlers;

public class EscapeTokenHandler : ITokenHandler
{
    public bool CanHandle(ReadOnlySpan<char> source, int index) =>
        source[index] == '\\';

    public int Handle(ReadOnlyMemory<char> source, int index, List<Token> tokens)
    {
        var span = source.Span;
        var length = source.Length;

        if (index + 1 < length)
        {
            var nextToken = span[index + 1];
            if (nextToken == '_' && index + 2 < length && span[index + 2] == '_')
            {
                tokens.Add(new Token(TokenKind.Text, source.Slice(index + 1, 2)));
                return 3;
            }

            if (IsSpecialCharacter(nextToken))
            {
                tokens.Add(new Token(TokenKind.Text, source.Slice(index + 1, 1)));
                return 2;
            }
        }

        tokens.Add(new Token(TokenKind.Text, source.Slice(index, 1)));
        return 1;
    }

    private static bool IsSpecialCharacter(char c) =>
        c is '#' or '_' or '\\' or '[' or ']' or '(' or ')';
}
