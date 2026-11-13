using System.Text;
using Markdown.Abstractions;
using Markdown.Models;

namespace Markdown.Parsers.MarkdownParsers;

public class UnorderedListParser(string text, StringBuilder buffer, List<Token> tokens)
    : TagParser(text, buffer, tokens)
{
    public bool IsListItemOpen { get; private set; }
    public bool IsUnorderedListOpen { get; private set; }

    public override bool TryParse(ref int currentPosition)
    {
        char? previous = currentPosition > 0 ? text[currentPosition - 1] : null;
        var current = text[currentPosition];
        char? next = currentPosition + 1 < text.Length ? text[currentPosition + 1] : null;

        if (TryOpen(previous, current, next, ref currentPosition))
            return true;

        CloseItemOnLineBreak(previous, current, next, currentPosition);

        return false;
    }

    private bool TryOpen(char? previous, char current, char? next, ref int currentPosition)
    {
        if (current != '-')
            return false;
        if (next != ' ')
            return false;
        if (!(previous == '\n' || currentPosition == 0))
            return false;

        FlushText();

        if (!IsUnorderedListOpen)
        {
            IsUnorderedListOpen = true;
            tokens.Add(new Token(TokenKind.UnorderedListStart));
        }

        tokens.Add(new Token(TokenKind.UnorderedListElementStart));
        IsListItemOpen = true;

        currentPosition++;
        return true;
    }

    private void CloseItemOnLineBreak(char? previous, char current, char? next, int currentPosition)
    {
        if (!IsListItemOpen)
            return;

        var isCrLf = current == '\r' && next == '\n';
        var isSingleLf = current == '\n' && previous != '\r';

        if (!(isCrLf || isSingleLf))
            return;

        FlushText();

        tokens.Add(new Token(TokenKind.UnorderedListElementEnd));
        IsListItemOpen = false;

        var nextLineStart = currentPosition + (isCrLf ? 2 : 1);
        var nextLineStartsWithListItem =
            nextLineStart + 1 < text.Length
            && text[nextLineStart] == '-'
            && text[nextLineStart + 1] == ' ';

        if (nextLineStartsWithListItem)
            return;

        IsUnorderedListOpen = false;
        tokens.Add(new Token(TokenKind.UnorderedListEnd));
    }
}