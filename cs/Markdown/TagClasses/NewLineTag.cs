namespace Markdown.TagClasses;

public class NewLineTag : Tag
{
    public override string Name => "NewLine";
    public override string MarkdownOpening => "\n";
    public override string MarkdownClosing => null;
    public override bool ShouldHavePair => true;
    public override bool TakePairTag => true;

    public override bool CanBeOpened(string markdownText, int startIndex)
    {
        return false;
    }

    public override bool IsMarkdownClosing(string markdownText, int endIndex)
    {
        return true;
    }

    public override bool CantBeInsideTags(IEnumerable<Tag> tagsContext)
    {
        return false;
    }
}