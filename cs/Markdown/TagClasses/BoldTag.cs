using Markdown.TagClasses.TagModels;

namespace Markdown.TagClasses;

public class BoldTag : Tag
{
    public BoldTag() : base(new BoldModel())
    {
    }

    public override bool CantBeInsideTags(IEnumerable<Tag> tagsContext)
    {
        return tagsContext.Select(tag => tag.GetType()).Contains(typeof(ItalicTag));
    }
}