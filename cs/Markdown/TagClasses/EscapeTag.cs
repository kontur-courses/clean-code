namespace Markdown.TagClasses;

public class EscapeTag : Tag
{
    public override string Name => "Escape";
    public override string MarkdownOpening => "\\";
    public override string MarkdownClosing => null;
    public override string HtmlTagOpen => null;
    public override string HtmlTagClose => null;
    public override bool ShouldHavePair => false;

    public override bool IsMarkdownOpening(string markdownText, int startIndex)
    {
        throw new NotImplementedException();
    }

    public override bool IsMarkdownClosing(string markdownText, int endIndex)
    {
        throw new NotImplementedException();
    }

    public override bool CanBeOpened(string markdownText, int startIndex)
    {
        throw new NotImplementedException();
    }

    public override bool CanBePairedWith(string markdownText, int currentTagStartIndex, Tag? otherTag, int otherTagStartIndex)
    {
        throw new NotImplementedException();
    }

    public override bool CantBeInsideTags(IEnumerable<Tag> tagsContext)
    {
        throw new NotImplementedException();
    }
}