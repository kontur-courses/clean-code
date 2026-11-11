using System.Text;

namespace Markdown.Tokens;

public class Tokenizer
{
    public IReadOnlyList<Token> Tokenize(string content)
    {
        var tokens = new List<Token>();
        var textBuffer = new StringBuilder();
        var index = 0;
        var noClosingParenBefore = -1;

        while (index < content.Length)
            switch (content[index])
            {
                case '\\':
                    index = AppendEscape(textBuffer, content, index);
                    break;
                case '_':
                    FlushText(tokens, textBuffer);
                    index = AddUnderscores(tokens, content, index);
                    break;
                case '[':
                    FlushText(tokens, textBuffer);
                    index = AddLeftBracket(tokens, content, index);
                    break;
                case ']':
                    FlushText(tokens, textBuffer);
                    index = AddRightBracket(tokens, content, index, ref noClosingParenBefore);
                    break;
                default:
                    index = AppendText(textBuffer, content, index);
                    break;
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

    private static int AddUnderscores(List<Token> tokens, string content, int start)
    {
        var end = start;
        while (end < content.Length && content[end] == '_')
            end++;

        var before = start > 0 ? content[start - 1] : Token.Boundary;
        var after = end < content.Length ? content[end] : Token.Boundary;

        for (var index = start; index < end;)
        {
            var isDouble = index + 1 < end;
            var kind = isDouble ? TokenKind.DoubleUnderscore : TokenKind.Underscore;
            var value = isDouble ? "__" : "_";

            tokens.Add(new Token(kind, value)
            {
                Before = before,
                After = after
            });

            index += value.Length;
        }

        return end;
    }

    private static int AddLeftBracket(List<Token> tokens, string content, int index)
    {
        tokens.Add(CreateMarkupToken(TokenKind.LeftBracket, "[", content, index));
        return index + 1;
    }

    private static int AddRightBracket(
        List<Token> tokens,
        string content,
        int index,
        ref int noClosingParenBefore)
    {
        tokens.Add(CreateMarkupToken(TokenKind.RightBracket, "]", content, index));
        var next = index + 1;

        if (next >= content.Length || content[next] != '(' || next < noClosingParenBefore)
            return next;

        var urlEnd = FindUrlEnd(content, next, out noClosingParenBefore);
        if (urlEnd < 0)
            return next;

        tokens.Add(CreateMarkupToken(TokenKind.LinkUrl, content[next..(urlEnd + 1)], content, next));
        return urlEnd + 1;
    }

    private static Token CreateMarkupToken(TokenKind kind, string value, string content, int start)
    {
        var end = start + value.Length;
        return new Token(kind, value)
        {
            Before = start > 0 ? content[start - 1] : Token.Boundary,
            After = end < content.Length ? content[end] : Token.Boundary
        };
    }

    private static int FindUrlEnd(string content, int urlStart, out int scannedUntil)
    {
        for (var index = urlStart + 1; index < content.Length; index++)
        {
            if (content[index] == '\n')
            {
                scannedUntil = index;
                return -1;
            }

            if (content[index] == ')')
            {
                scannedUntil = index;
                return index;
            }
        }

        scannedUntil = content.Length;
        return -1;
    }

    private static bool IsSpecial(char symbol)
    {
        return symbol is '\\' or '_' or '[' or ']';
    }
}