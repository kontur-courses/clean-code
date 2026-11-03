namespace Markdown.Parsing;

// обработка заголовков, абзацев и списков
internal sealed class BlockParser
{
    private readonly InlineParser inlineParser;

    public BlockParser(InlineParser inlineParser)
    {
        this.inlineParser = inlineParser;
    }

    public string Parse(string text)
    {
        throw new NotImplementedException();
    }

    private IReadOnlyList<string> SplitLines(string text)
    {
        throw new NotImplementedException();
    }

    private bool IsHeading(string line)
    {
        throw new NotImplementedException();
    }

    private bool IsUnorderedListItem(string line)
    {
        throw new NotImplementedException();
    }

    private string ParseHeading(string line)
    {
        throw new NotImplementedException();
    }

    private string ParseParagraph(string text)
    {
        throw new NotImplementedException();
    }

    private string ParseUnorderedList(IReadOnlyList<string> items)
    {
        throw new NotImplementedException();
    }
}