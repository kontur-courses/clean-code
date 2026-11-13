using System.Text;
using Markdown.Abstractions;
using Markdown.Models;

namespace Markdown.Parsers.MarkdownParsers;

public class EscapeParser(string text, StringBuilder buffer, List<Token> tokens) : TagParser(text, buffer, tokens)
{
    public override bool TryParse(ref int currentPosition)
    {
        var current = text[currentPosition];
        char? next = currentPosition + 1 < text.Length ? text[currentPosition + 1] : null;

        return TryProcessEscape(current, next, ref currentPosition);
    }


    private bool TryProcessEscape(char current, char? next, ref int i)
    {
        if (current != '\\')
            return false;

        if (next is not ('_' or '\\'))
            return false;

        buffer.Append(next);
        i++;
        return true;
    }
}