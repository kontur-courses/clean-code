using System.Text;
using Markdown.Abstractions;
using Markdown.Models;

namespace Markdown.Parsers.MarkdownParsers;

public class EmphasisParser(string text, StringBuilder buffer, List<Token> tokens) : TagParser(text, buffer, tokens)
{
    public int EmphasisOpeningTokenIndex = -1;
    public bool IsEmphasisInvalidatedByIntersection;
    private bool emphasisContainsWhitespace;
    private bool emphasisStartsInsideWord;
    
    public override bool TryParse(ref int currentPosition)
    {
        char? previous = currentPosition > 0 ? text[currentPosition - 1] : null;
        var current = text[currentPosition];
        char? next = currentPosition + 1 < text.Length ? text[currentPosition + 1] : null;
        
        if (EmphasisOpeningTokenIndex != -1 
            && char.IsWhiteSpace(current))
            emphasisContainsWhitespace = true;
        
        if (current != '_')
            return false;

        var isOpened = TryOpenEmphasis(previous, next);

        return isOpened || TryCloseEmphasis(previous, next);
    }

    private bool TryOpenEmphasis(char? previous, char? next)
    {
        if (EmphasisOpeningTokenIndex != -1)
            return false;

        if (char.IsWhiteSpace(next ?? ' '))
            return false;
        if (char.IsDigit(next ?? ' '))
            return false;

        FlushText();

        EmphasisOpeningTokenIndex = tokens.Count;
        tokens.Add(new Token(TokenKind.Text, "_"));

        emphasisStartsInsideWord = char.IsLetterOrDigit(previous ?? ' ');
        emphasisContainsWhitespace = false;

        return true;
    }

    private bool TryCloseEmphasis(char? previous, char? next)
    {
        if (EmphasisOpeningTokenIndex == -1)
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

        if (IsEmphasisInvalidatedByIntersection || spansDifferentWords)
        {
            buffer.Append('_');
            ResetEmphasisState();
            return true;
        }

        FlushText();

        tokens[EmphasisOpeningTokenIndex] = new Token(TokenKind.EmphasisStart);
        tokens.Add(new Token(TokenKind.EmphasisEnd));

        ResetEmphasisState();

        return true;
    }

    private void ResetEmphasisState()
    {
        EmphasisOpeningTokenIndex = -1;
        IsEmphasisInvalidatedByIntersection = false;
        emphasisStartsInsideWord = false;
        emphasisContainsWhitespace = false;
    }
}