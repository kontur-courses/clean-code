using System.Text;
using Markdown.Abstractions;
using Markdown.Models;

namespace Markdown.Parsers.MarkdownParsers;

public class StrongParser(string text, StringBuilder buffer, List<Token> tokens) : TagParser(text, buffer, tokens)
{
    public int StrongOpeningTokenIndex = -1;
    public bool TryParse(ref int currentPosition, ref int emphasisOpeningTokenIndex, ref bool isEmphasisInvalidatedByIntersection)
    {
        char? previous = currentPosition > 0 ? text[currentPosition - 1] : null;
        var current = text[currentPosition];
        char? next = currentPosition + 1 < text.Length ? text[currentPosition + 1] : null;
        char? nextNext = currentPosition + 2 < text.Length ? text[currentPosition + 2] : null;
        
        if (!(current == '_' && next == '_'))
            return false;

        var isOpened = TryOpenStrong(nextNext, ref currentPosition, ref emphasisOpeningTokenIndex);

        return isOpened || TryCloseStrong(previous, ref currentPosition, ref emphasisOpeningTokenIndex, ref isEmphasisInvalidatedByIntersection);
    }
    
    private bool TryOpenStrong(char? afterDoubleUnderscore, ref int i, ref int emphasisOpeningTokenIndex)
    {
        if (StrongOpeningTokenIndex != -1)
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

        StrongOpeningTokenIndex = tokens.Count;
        tokens.Add(new Token(TokenKind.Text, "__"));

        i++;
        return true;
    }

    private bool TryCloseStrong(char? previous, ref int i, ref int emphasisOpeningTokenIndex, ref bool isEmphasisInvalidatedByIntersection)
    {
        if (StrongOpeningTokenIndex == -1)
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
            StrongOpeningTokenIndex = -1;
            buffer.Append("__");
            i++;
            return true;
        }

        FlushText();

        tokens[StrongOpeningTokenIndex] = new Token(TokenKind.StrongStart);
        StrongOpeningTokenIndex = -1;
        tokens.Add(new Token(TokenKind.StrongEnd));

        i++;
        return true;
    }

    public override bool TryParse(ref int currentPosition)
    {
        throw new NotImplementedException();
    }
}