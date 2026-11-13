using System.Text;
using Markdown.Abstractions;
using Markdown.Models;

namespace Markdown.Parsers.MarkdownParsers;

public sealed class HeaderParser(string text, StringBuilder buffer, List<Token> tokens) : TagParser(text, buffer, tokens)
{
    public bool IsHeadingOpen { get; private set; }
    
    public override bool TryParse(ref int currentPosition)
    {
        char? previous = currentPosition > 0 ? text[currentPosition - 1] : null;
        var current = text[currentPosition];
        char? next = currentPosition + 1 < text.Length ? text[currentPosition + 1] : null;

        var isOpened = TryOpenHeading(previous, current, next, ref currentPosition);

        CloseHeadingOnLineBreak(current, next);

        return isOpened;
    }
        
    private bool TryOpenHeading(char? previous, char current, char? next, ref int i)
    {
        if (current != '#')
            return false;

        if (next != ' ')
            return false;

        if (!(i == 0 || previous == '\n'))
            return false;

        FlushText();

        IsHeadingOpen = true;
        tokens.Add(new Token(TokenKind.HeadingStart));

        i++;
        return true;
    }

    private void CloseHeadingOnLineBreak(char current, char? next)
    {
        if (!IsHeadingOpen)
            return;

        if (!(current == '\n' || (current == '\r' && next == '\n')))
            return;

        FlushText();

        tokens.Add(new Token(TokenKind.HeadingEnd));
        IsHeadingOpen = false;
    }
}