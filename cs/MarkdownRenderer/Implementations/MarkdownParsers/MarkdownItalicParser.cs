using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.MarkdownParsers;

public class MarkdownItalicParser : MarkdownElementParser<ItalicElement>
{
    public override ElementParseType ParseType => ElementParseType.Inline;
    private const char Special = '_';

    public override bool IsElementStart(string content, int index)
    {
        return
            content[index] is Special &&
            index + 1 < content.Length &&
            content[index + 1] is not (' ' or '_');
    }

    public override bool IsElementEnd(string content, int index)
    {
        return content[index] is Special &&
            index > 0 &&
            content[index - 1] is not (' ' or '_') &&
            (index + 1 >= content.Length || content[index + 1] is not '_');
    }

    public override bool TryParseElement(string content, Token contentToken, out ItalicElement? element)
    {
        element = null;
        if (!IsElementStart(content, contentToken.Start) || !IsElementEnd(content, contentToken.End))
            return false;
        if (contentToken.Length < 3)
            return false;

        var rawContent = content.Substring(contentToken.Start + 1, contentToken.Length - 2);
        if (
            IsSelectionInsideWord(content, contentToken) &&
            rawContent.Any(symbol => char.IsDigit(symbol) || symbol == ' ')
        )
            return false;

        element = new ItalicElement(rawContent);
        return true;
    }

    private static bool IsSelectionInsideWord(string content, Token contentToken)
    {
        if (contentToken.Start > 0 && content[contentToken.Start - 1] != ' ')
            return true;

        if (contentToken.End + 1 < content.Length && content[contentToken.End + 1] != ' ')
            return true;

        return false;
    }
}