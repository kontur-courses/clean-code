using Markdown.Core.Lexing;

namespace Markdown.Core.Parsing;

public class InlineValidator
{
    public bool IsValidEmphasisClose(IReadOnlyList<Token> tokens, int startIndex, int closeIndex)
    {
        if (!HasValidOpeningBoundary(tokens, startIndex) ||
            !HasValidClosingBoundary(tokens, closeIndex))
            return false;
        if (startIndex + 1 == closeIndex)
            return false;
        if (IsInDigitContext(tokens, startIndex) || IsInDigitContext(tokens, closeIndex))
            return false;
        if (HasIntersectingDoubleInsideEmphasis(tokens, startIndex, closeIndex))
            return false;
        if (IsInsideWord(tokens, startIndex) && IsInsideWord(tokens, closeIndex) &&
            ContainsWhitespaceBetween(tokens, startIndex, closeIndex))
            return false;

        return true;
    }

    public bool IsValidStrongClose(IReadOnlyList<Token> tokens, int startIndex, int closeIndex)
    {
        if (!HasValidOpeningBoundary(tokens, startIndex) ||
            !HasValidClosingBoundary(tokens, closeIndex))
            return false;
        if (startIndex + 1 == closeIndex)
            return false;
        if (IsInDigitContext(tokens, startIndex) || IsInDigitContext(tokens, closeIndex))
            return false;
        if (HasIntersectingDoubleUnderscore(tokens, startIndex, closeIndex))
            return false;
        if (HasIntersectingSingleInsideStrong(tokens, startIndex, closeIndex))
            return false;
        if (IsInsideWord(tokens, startIndex) && IsInsideWord(tokens, closeIndex) &&
            ContainsWhitespaceBetween(tokens, startIndex, closeIndex))
            return false;

        return true;
    }

    private static bool HasValidOpeningBoundary(IReadOnlyList<Token> tokens, int startIndex)
    {
        if (startIndex + 1 >= tokens.Count)
            return true;

        var next = tokens[startIndex + 1];
        return next.Kind is not TokenKind.Space and not TokenKind.NewLine;
    }

    private static bool HasValidClosingBoundary(IReadOnlyList<Token> tokens, int closeIndex)
    {
        if (closeIndex - 1 < 0)
            return true;

        var prev = tokens[closeIndex - 1];
        if (prev.Kind != TokenKind.Space)
            return true;

        if (closeIndex + 1 >= tokens.Count)
            return true;

        var next = tokens[closeIndex + 1];
        return next.Kind is TokenKind.Space or TokenKind.NewLine or TokenKind.Eof;
    }

    private static bool IsInDigitContext(IReadOnlyList<Token> tokens, int index) =>
        HasDigitBefore(tokens, index) || HasDigitAfter(tokens, index);

    private static bool HasDigitBefore(IReadOnlyList<Token> tokens, int index)
    {
        if (index == 0)
            return false;

        var prev = tokens[index - 1];
        return prev.Kind == TokenKind.Text &&
               prev.Slice.Length > 0 &&
               char.IsDigit(prev.Slice.Span[^1]);
    }

    private static bool HasDigitAfter(IReadOnlyList<Token> tokens, int index)
    {
        if (index + 1 >= tokens.Count)
            return false;

        var next = tokens[index + 1];
        return next.Kind == TokenKind.Text &&
               next.Slice.Length > 0 &&
               char.IsDigit(next.Slice.Span[0]);
    }

    private static bool HasIntersectingDoubleInsideEmphasis(IReadOnlyList<Token> tokens, int startIndex, int closeIndex)
    {
        var pending = false;

        for (var i = startIndex + 1; i < closeIndex; i++)
        {
            if (tokens[i].Kind != TokenKind.DoubleUnderscore)
                continue;

            pending = !pending;
        }

        return pending;
    }

    private static bool HasIntersectingSingleInsideStrong(IReadOnlyList<Token> tokens, int startIndex, int closeIndex)
    {
        var pending = false;

        for (var i = startIndex + 1; i < closeIndex; i++)
        {
            if (tokens[i].Kind != TokenKind.Underscore)
                continue;

            pending = !pending;
        }

        return pending;
    }

    private static bool HasIntersectingDoubleUnderscore(IReadOnlyList<Token> tokens, int startIndex, int closeIndex)
    {
        for (var i = startIndex + 1; i < closeIndex; i++)
            if (tokens[i].Kind == TokenKind.DoubleUnderscore)
                return true;
        return false;
    }

    private bool IsInsideWord(IReadOnlyList<Token> tokens, int index) =>
        HasLetterOrDigitBefore(tokens, index) && HasLetterOrDigitAfter(tokens, index);

    private static bool HasLetterOrDigitBefore(IReadOnlyList<Token> tokens, int index)
    {
        if (index == 0)
            return false;

        var prev = tokens[index - 1];
        return prev.Kind == TokenKind.Text &&
               prev.Slice.Length > 0 &&
               char.IsLetterOrDigit(prev.Slice.Span[^1]);
    }

    private static bool HasLetterOrDigitAfter(IReadOnlyList<Token> tokens, int index)
    {
        if (index + 1 >= tokens.Count)
            return false;

        var next = tokens[index + 1];
        return next.Kind == TokenKind.Text &&
               next.Slice.Length > 0 &&
               char.IsLetterOrDigit(next.Slice.Span[0]);
    }

    private static bool ContainsWhitespaceBetween(IReadOnlyList<Token> tokens, int startIndex, int closeIndex)
    {
        for (var i = startIndex + 1; i < closeIndex; i++)
            if (tokens[i].Kind == TokenKind.Space)
                return true;
        return false;
    }
}
