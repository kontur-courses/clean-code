using System.Text;

namespace Markdown.Tokens;

public class Tokenizer
{
    public IReadOnlyList<Token> Tokenize(string content)
    {
        var tokens = new List<Token>();
        var textBuffer = new StringBuilder();
        var textStart = -1;
        var openBracketCount = 0;
        var index = 0;

        while (index < content.Length)
        {
            switch (content[index])
            {
                case '\\':
                    AppendEscape(textBuffer, ref textStart, content, ref index);
                    break;
                case '_':
                    FlushText(tokens, textBuffer, ref textStart, content, index);
                    AddUnderscore(tokens, content, ref index);
                    break;
                case '[':
                    FlushText(tokens, textBuffer, ref textStart, content, index);
                    AddLeftBracket(tokens, content, ref index, ref openBracketCount);
                    break;
                case ']':
                    FlushText(tokens, textBuffer, ref textStart, content, index);
                    AddRightBracket(tokens, content, ref index, ref openBracketCount);
                    break;
                default:
                    AppendText(textBuffer, ref textStart, content, ref index);
                    break;
            }
        }

        FlushText(tokens, textBuffer, ref textStart, content, content.Length);
        return tokens;
    }

    private static void AppendText(StringBuilder textBuffer, ref int textStart, string content, ref int index)
    {
        if (textStart < 0)
            textStart = index;

        var start = index;
        while (index < content.Length && !IsSpecial(content[index]))
            index++;

        textBuffer.Append(content, start, index - start);
    }

    private static void AppendEscape(
        StringBuilder textBuffer,
        ref int textStart,
        string content,
        ref int index)
    {
        if (textStart < 0)
            textStart = index;

        var start = index;
        var next = start + 1;

        if (next < content.Length && IsSpecial(content[next]))
        {
            textBuffer.Append(content[next]);
            index = next + 1;
            return;
        }

        textBuffer.Append('\\');
        index = next;
    }

    private static void FlushText(
        List<Token> tokens,
        StringBuilder textBuffer,
        ref int textStart,
        string content,
        int currentIndex)
    {
        if (textBuffer.Length == 0)
            return;

        tokens.Add(new Token(TokenKind.Text, textBuffer.ToString(), textStart)
        {
            Before = textStart > 0 ? content[textStart - 1] : Token.Boundary,
            After = currentIndex < content.Length ? content[currentIndex] : Token.Boundary
        });

        textBuffer.Clear();
        textStart = -1;
    }

    private static void AddUnderscore(List<Token> tokens, string content, ref int index)
    {
        var start = index;
        var isDouble = start + 1 < content.Length && content[start + 1] == '_';
        var length = isDouble ? 2 : 1;

        tokens.Add(new Token(
            isDouble ? TokenKind.DoubleUnderscore : TokenKind.Underscore,
            isDouble ? "__" : "_",
            start)
        {
            Before = start > 0 ? content[start - 1] : Token.Boundary,
            After = start + length < content.Length ? content[start + length] : Token.Boundary
        });

        index += length;
    }

    private static void AddLeftBracket(
        List<Token> tokens,
        string content,
        ref int index,
        ref int openBracketCount)
    {
        tokens.Add(new Token(TokenKind.LeftBracket, "[", index)
        {
            Before = index > 0 ? content[index - 1] : Token.Boundary,
            After = index + 1 < content.Length ? content[index + 1] : Token.Boundary
        });

        openBracketCount++;
        index++;
    }

    private static void AddRightBracket(
        List<Token> tokens,
        string content,
        ref int index,
        ref int openBracketCount)
    {
        var start = index;
        var urlStart = start + 1;

        tokens.Add(new Token(TokenKind.RightBracket, "]", start)
        {
            Before = start > 0 ? content[start - 1] : Token.Boundary,
            After = urlStart < content.Length ? content[urlStart] : Token.Boundary
        });

        var hadOpenBracket = openBracketCount > 0;
        if (hadOpenBracket)
            openBracketCount--;

        index = urlStart;

        if (!hadOpenBracket || urlStart >= content.Length || content[urlStart] != '(')
            return;

        var urlEnd = FindUrlEnd(content, urlStart);
        if (urlEnd < 0)
            return;

        tokens.Add(new Token(TokenKind.LinkUrl, content[urlStart..(urlEnd + 1)], urlStart)
        {
            Before = content[urlStart - 1],
            After = urlEnd + 1 < content.Length ? content[urlEnd + 1] : Token.Boundary
        });

        index = urlEnd + 1;
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
