using System.Text;
using Markdown.Abstractions;
using Markdown.Models;

namespace Markdown.Parsers.MarkdownParsers;

public class EmphasisParser(string text, StringBuilder buffer, List<Token> tokens) : TagParser(text, buffer, tokens)
{
    private bool emphasisContainsWhitespace;
    private int emphasisOpeningTokenIndex = -1;
    private bool emphasisStartsInsideWord;
    private bool isEmphasisInvalidatedByIntersection;
    private int strongOpeningTokenIndex = -1;

    public override bool TryParse(ref int currentPosition)
    {
        char? previous = currentPosition > 0 ? text[currentPosition - 1] : null;
        var current = text[currentPosition];
        char? next = currentPosition + 1 < text.Length ? text[currentPosition + 1] : null;
        char? nextNext = currentPosition + 2 < text.Length ? text[currentPosition + 2] : null;

        if (emphasisOpeningTokenIndex != -1
            && char.IsWhiteSpace(current))
            emphasisContainsWhitespace = true;

        var isStrong = TryProcessStrong(previous, current, next, nextNext, ref currentPosition);

        return isStrong || TryProcessEmphasis(previous, current, next);
    }

    private bool TryProcessStrong(char? previous, char current, char? next, char? afterDoubleUnderscore,
        ref int currentPosition)
    {
        if (!(current == '_' && next == '_'))
            return false;

        var isOpened = TryOpenStrong(afterDoubleUnderscore, ref currentPosition);

        return isOpened || TryCloseStrong(previous, ref currentPosition);
    }

    private bool TryProcessEmphasis(char? previous, char current, char? next)
    {
        if (current != '_')
            return false;

        var isOpened = TryOpenEmphasis(previous, next);

        return isOpened || TryCloseEmphasis(previous, next);
    }

    private bool TryOpenStrong(char? afterDoubleUnderscore, ref int i)
    {
        if (strongOpeningTokenIndex != -1)
            return false;

        if (char.IsWhiteSpace(afterDoubleUnderscore ?? ' ')
            || afterDoubleUnderscore == '_'
            || emphasisOpeningTokenIndex != -1)
        {
            buffer.Append("__");
            i++;
            return true;
        }

        FlushText();

        strongOpeningTokenIndex = tokens.Count;
        tokens.Add(new Token(TokenKind.Text, "__"));

        i++;
        return true;
    }

    private bool TryCloseStrong(char? previous, ref int i)
    {
        if (strongOpeningTokenIndex == -1)
            return false;

        if (char.IsWhiteSpace(previous ?? ' '))
        {
            buffer.Append("__");
            i++;
            return true;
        }

        if (emphasisOpeningTokenIndex != -1)
        {
            isEmphasisInvalidatedByIntersection = true;
            strongOpeningTokenIndex = -1;
            buffer.Append("__");
            i++;
            return true;
        }

        FlushText();

        tokens[strongOpeningTokenIndex] = new Token(TokenKind.StrongStart);
        strongOpeningTokenIndex = -1;
        tokens.Add(new Token(TokenKind.StrongEnd));

        i++;
        return true;
    }

    private bool TryOpenEmphasis(char? previous, char? next)
    {
        if (emphasisOpeningTokenIndex != -1)
            return false;

        if (char.IsWhiteSpace(next ?? ' '))
            return false;
        if (char.IsDigit(next ?? ' '))
            return false;

        FlushText();

        emphasisOpeningTokenIndex = tokens.Count;
        tokens.Add(new Token(TokenKind.Text, "_"));

        emphasisStartsInsideWord = char.IsLetterOrDigit(previous ?? ' ');
        emphasisContainsWhitespace = false;

        return true;
    }

    private bool TryCloseEmphasis(char? previous, char? next)
    {
        if (emphasisOpeningTokenIndex == -1)
            return false;

        if (char.IsWhiteSpace(previous ?? ' '))
            return false;
        if (char.IsDigit(previous ?? ' '))
            return false;

        var endsInsideWord = char.IsLetterOrDigit(next ?? ' ');

        var spansDifferentWords =
            emphasisStartsInsideWord
            && endsInsideWord
            && emphasisContainsWhitespace;

        if (isEmphasisInvalidatedByIntersection || spansDifferentWords)
        {
            buffer.Append('_');
            ResetEmphasisState();
            return true;
        }

        FlushText();

        tokens[emphasisOpeningTokenIndex] = new Token(TokenKind.EmphasisStart);
        tokens.Add(new Token(TokenKind.EmphasisEnd));

        ResetEmphasisState();

        return true;
    }

    private void ResetEmphasisState()
    {
        emphasisOpeningTokenIndex = -1;
        isEmphasisInvalidatedByIntersection = false;
        emphasisStartsInsideWord = false;
        emphasisContainsWhitespace = false;
    }

    internal void ResetPendingMarkers()
    {
        ResetEmphasisState();
        strongOpeningTokenIndex = -1;
    }
}