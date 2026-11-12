using Markdown.Tokenizer.Tokens;
using Markdown.Tokenizer.Interfaces;
using System.Text;

namespace Markdown.Tokenizer;

public class MarkdownTokenizer : ITokenizer
{
    public IReadOnlyList<Token> Tokenize(string text)
    {
        var tokens = new List<Token>();
        var textBuffer = new StringBuilder();
        var i = 0;

        while (i < text.Length)
        {
            var current = text[i];

            if (current == '\\')
            {
                if (i + 1 < text.Length && IsEscapable(text[i + 1]))
                {
                    textBuffer.Append(text[i + 1]);
                    i += 2;
                }
                else
                {
                    textBuffer.Append('\\');
                    i++;
                }
                continue;
            }

            if (char.IsWhiteSpace(current))
            {
                FlushTextBuffer(tokens, textBuffer);
                var start = i;
                while (i < text.Length && char.IsWhiteSpace(text[i]))
                    i++;

                tokens.Add(new Token(TokenType.Whitespace, text[start..i]));
                continue;
            }

            if (current == '_')
            {
                var start = i;
                while (i < text.Length && text[i] == '_')
                    i++;

                var count = i - start;

                if (count > 2)
                {
                    textBuffer.Append(text, start, count);
                    continue;
                }

                FlushTextBuffer(tokens, textBuffer);
                tokens.Add(CreateUnderscoreToken(text, start, i));
                continue;
            }

            if (TryGetBracketTokenType(current, out var bracketType))
            {
                FlushTextBuffer(tokens, textBuffer);
                tokens.Add(new Token(bracketType, current.ToString()));
                i++;
                continue;
            }

            textBuffer.Append(current);
            i++;
        }

        FlushTextBuffer(tokens, textBuffer);
        return tokens;
    }

    private static Token CreateUnderscoreToken(string text, int start, int end)
    {
        var length = end - start;
        var type = length == 1 ? TokenType.SingleUnderscore : TokenType.DoubleUnderscore;

        var hasDigitNeighbor =
            (start > 0 && char.IsDigit(text[start - 1]) && end < text.Length && !char.IsWhiteSpace(text[end])) ||
            (end < text.Length && char.IsDigit(text[end]) && start > 0 && !char.IsWhiteSpace(text[start - 1]));

        var prevIsNonWhitespace = start > 0 && !char.IsWhiteSpace(text[start - 1]);
        var nextIsNonWhitespace = end < text.Length && !char.IsWhiteSpace(text[end]);

        var prevIsLetterOrDigit = start > 0 && char.IsLetterOrDigit(text[start - 1]);
        var nextIsLetterOrDigit = end < text.Length && char.IsLetterOrDigit(text[end]);

        var isInsideWord = prevIsLetterOrDigit && nextIsLetterOrDigit;
        var canOpen = !hasDigitNeighbor && nextIsNonWhitespace;
        var canClose = !hasDigitNeighbor && prevIsNonWhitespace;

        return new Token(type, text[start..end], canOpen, canClose, isInsideWord);
    }

    private static void FlushTextBuffer(List<Token> tokens, StringBuilder buffer)
    {
        if (buffer.Length == 0)
            return;

        tokens.Add(new Token(TokenType.Text, buffer.ToString()));
        buffer.Clear();
    }

    private static bool IsEscapable(char c) =>
        c is '\\' or '_' or '#' or '[' or ']' or '(' or ')';

    private static bool TryGetBracketTokenType(char c, out TokenType type)
    {
        switch (c)
        {
            case '[':
                type = TokenType.OpenBracket;
                return true;
            case ']':
                type = TokenType.CloseBracket;
                return true;
            case '(':
                type = TokenType.OpenParen;
                return true;
            case ')':
                type = TokenType.CloseParen;
                return true;
            default:
                type = default;
                return false;
        }
    }
}