namespace Markdown.Core.Lexing.Handlers;

public class TextTokenHandler : ITokenHandler
{
    public bool CanHandle(ReadOnlySpan<char> source, int index) => true;

    public int Handle(ReadOnlyMemory<char> source, int index, List<Token> tokens)
    {
        var span = source.Span;
        var length = source.Length;
        var startText = index;

        while (index < length)
        {
            var symbol = span[index];
            if (IsSpecialCharacter(symbol) || symbol is ' ' or '\t' or '\n' or '\r')
                break;
            index++;
        }

        if (index > startText)
        {
            tokens.Add(new Token(TokenKind.Text, source.Slice(startText, index - startText)));
            return index - startText;
        }
        
        tokens.Add(new Token(TokenKind.Text, source.Slice(startText, 1)));
        return 1;
    }

    private static bool IsSpecialCharacter(char c) =>
        c is '#' or '_' or '\\' or '[' or ']' or '(' or ')';
}
