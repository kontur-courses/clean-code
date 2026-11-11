using System.Text;

namespace Markdown.Tokens;

public class Tokenizer
{
    public IReadOnlyList<Token> Tokenize(string content)
    {
        var tokens = new List<Token>();
        var textBuffer = new StringBuilder();
        var index = 0;

        while (index < content.Length)
        {
            switch (content[index])
            {
                case '\\':
                    index = AppendEscape(textBuffer, content, index);
                    break;
                case '_':
                    FlushText(tokens, textBuffer);
                    index = AddUnderscore(tokens, content, index);
                    break;
                case '[':
                    FlushText(tokens, textBuffer);
                    index = AddLeftBracket(tokens, content, index);
                    break;
                case ']':
                    FlushText(tokens, textBuffer);
                    index = AddRightBracket(tokens, content, index);
                    break;
                default:
                    index = AppendText(textBuffer, content, index);
                    break;
            }
        }

        FlushText(tokens, textBuffer);
        return tokens;
    }

    private static int AppendText(StringBuilder textBuffer, string content, int index)
    {
        var start = index;
        while (index < content.Length && !IsSpecial(content[index]))
            index++;

        textBuffer.Append(content, start, index - start);
        return index;
    }

    private static int AppendEscape(StringBuilder textBuffer, string content, int index)
    {
        var next = index + 1;

        if (next < content.Length && IsSpecial(content[next]))
        {
            textBuffer.Append(content[next]);
            return next + 1;
        }

        textBuffer.Append('\\');
        return next;
    }

    private static void FlushText(List<Token> tokens, StringBuilder textBuffer)
    {
        if (textBuffer.Length == 0)
            return;

        tokens.Add(new Token(TokenKind.Text, textBuffer.ToString()));
        textBuffer.Clear();
    }

    private static int AddUnderscore(List<Token> tokens, string content, int index)
    {
        var isDouble = index + 1 < content.Length && content[index + 1] == '_';
        var token = isDouble
            ? CreateMarkup(TokenKind.DoubleUnderscore, "__", content, index)
            : CreateMarkup(TokenKind.Underscore, "_", content, index);

        tokens.Add(token);
        return index + token.Value.Length;
    }

    private static int AddLeftBracket(List<Token> tokens, string content, int index)
    {
        tokens.Add(CreateMarkup(TokenKind.LeftBracket, "[", content, index));
        return index + 1;
    }

    private static int AddRightBracket(List<Token> tokens, string content, int index)
    {
        tokens.Add(CreateMarkup(TokenKind.RightBracket, "]", content, index));
        var next = index + 1;

        if (next >= content.Length || content[next] != '(')
            return next;

        var urlEnd = FindUrlEnd(content, next);
        if (urlEnd < 0)
            return next;

        tokens.Add(CreateMarkup(TokenKind.LinkUrl, content[next..(urlEnd + 1)], content, next));
        return urlEnd + 1;
    }

    private static Token CreateMarkup(TokenKind kind, string value, string content, int start)
    {
        var end = start + value.Length;
        return new Token(kind, value)
        {
            Before = start > 0 ? content[start - 1] : Token.Boundary,
            After = end < content.Length ? content[end] : Token.Boundary
        };
    }

    private static int FindUrlEnd(string content, int urlStart)
    {
        for (var index = urlStart + 1; index < content.Length; index++)
        {
            if (content[index] == '\n')
                return -1;
            if (content[index] == ')')
                return index;
        }

        return -1;
    }

    private static bool IsSpecial(char symbol)
    {
        return symbol is '\\' or '_' or '[' or ']';
    }
}
