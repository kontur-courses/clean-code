using System.Text;

namespace Markdown.Parsing;

/// <summary>
/// Превращает исходный текст в плоский список токенов, а
/// экранированные символы становятся обычным текстом
/// </summary>
internal sealed class MarkdownLexer
{
    // маркер состоит из символа и пробела с ним
    private const int MarkerLength = 2;

    private readonly string source;
    private readonly List<Token> tokens = new();
    private readonly StringBuilder text = new();
    private int position;

    private MarkdownLexer(string source)
    {
        this.source = source;
    }

    public static IReadOnlyList<Token> Tokenize(string source)
    {
        ArgumentNullException.ThrowIfNull(source);

        return new MarkdownLexer(source).ReadTokens();
    }

    private bool IsAtLineStart =>
        position == 0 || IsLineBreak(source[position - 1]);

    private IReadOnlyList<Token> ReadTokens()
    {
        while (position < source.Length)
        {
            ReadNext();
        }

        FlushText();

        return tokens;
    }

    private void ReadNext()
    {
        var current = source[position];

        if (current == '\\')
            ReadEscapedCharacter();
        else if (IsLineBreak(current))
            ReadLineBreak();
        else if (current == '_')
            ReadUnderscoreRun();
        else if (!TryReadLineStartMarker())
            ReadText();
    }

    private void ReadEscapedCharacter()
    {
        var escapedPosition = position + 1;

        if (escapedPosition >= source.Length)
        {
            // обратный слэшь в конце текста ничего не экранирует
            text.Append('\\');
            position++;
            return;
        }

        var escaped = source[escapedPosition];

        if (IsLineBreak(escaped))
        {
            text.Append('\n');
            position = escapedPosition + GetLineBreakLength(escapedPosition);
            return;
        }

        text.Append(escaped);
        position = escapedPosition + 1;
    }

    private void ReadLineBreak()
    {
        FlushText();

        tokens.Add(Token.LineBreak);
        position += GetLineBreakLength(position);
    }

    private void ReadUnderscoreRun()
    {
        FlushText();

        var start = position;

        while (position < source.Length && source[position] == '_')
        {
            position++;
        }

        tokens.Add(new Token(TokenKind.UnderscoreRun, source[start..position]));
    }

    private bool TryReadLineStartMarker()
    {
        var hasSpaceAfter =
            position + 1 < source.Length && source[position + 1] == ' ';

        if (!IsAtLineStart ||
            !hasSpaceAfter ||
            !TryGetMarkerKind(source[position], out var kind))
        {
            return false;
        }

        FlushText();

        tokens.Add(new Token(
            kind,
            source.Substring(position, MarkerLength)));
        position += MarkerLength;

        return true;
    }

    private void ReadText()
    {
        var start = position;

        // первый символ входит в текст всегда, это может быть - или #
        // в начале строки, которые не оказались маркерами
        position++;

        // - и # в середине строки обычные, поэтому текст они не прерывают
        while (position < source.Length &&
               source[position] is not ('\\' or '_' or '\r' or '\n'))
        {
            position++;
        }

        text.Append(source, start, position - start);
    }

    private void FlushText()
    {
        if (text.Length == 0)
            return;

        tokens.Add(new Token(TokenKind.Text, text.ToString()));
        text.Clear();
    }

    private static bool TryGetMarkerKind(char character, out TokenKind kind)
    {
        switch (character)
        {
            case '-':
                kind = TokenKind.ListMarker;
                return true;

            case '#':
                kind = TokenKind.HeadingMarker;
                return true;

            default:
                kind = default;
                return false;
        }
    }

    private static bool IsLineBreak(char character)
    {
        return character is '\r' or '\n';
    }

    private int GetLineBreakLength(int index)
    {
        return source[index] == '\r' &&
               index + 1 < source.Length &&
               source[index + 1] == '\n'
            ? 2
            : 1;
    }
}
