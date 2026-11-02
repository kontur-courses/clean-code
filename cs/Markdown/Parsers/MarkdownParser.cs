using Markdown.Entities;

namespace Markdown.Parsers;

public sealed class MarkdownParser
{
    public IEnumerable<MarkdownBlock> Parse(string markdownText)
    {
        throw new NotImplementedException();
    }

    private IEnumerable<string> SplitIntoBlocks(string markdownText)
    {
        throw new NotImplementedException();
    }

    private MarkdownBlock ParseBlock(string markdownBlockText)
    {
        throw new NotImplementedException();
    }
    
    private bool IsHeading(string markdownBlockText)
    {
        throw new NotImplementedException();
    }

    private MarkdownBlock CreateHeadingBlock(string markdownBlockText)
    {
        throw new NotImplementedException();
    }

    private MarkdownBlock CreateParagraphBlock(string markdownBlockText)
    {
        throw new NotImplementedException();
    }
}