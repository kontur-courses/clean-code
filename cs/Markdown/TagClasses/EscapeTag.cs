using Markdown.TagClasses.TagModels;

namespace Markdown.TagClasses;

public class EscapeTag : Tag
{
    public EscapeTag() : base(new EscapeModel())
    {
    }

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

    public override bool CanBePairedWith(string markdownText, int currentTagStartIndex, Tag? otherTag, int otherTagEndIndex)
    {
        throw new NotImplementedException();
    }

    public override bool CantBeInsideTags(IEnumerable<Tag> tagsContext)
    {
        throw new NotImplementedException();
    }
}