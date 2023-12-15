using MarkDown.Abstracts;
using MarkDown.Enums;
using MarkDown.TagCreators;

namespace MarkDown;

public class MarkDownEnvironment
{
    private readonly Dictionary<TagName, Tag> markDownTags;
    private readonly Dictionary<Tag, List<TagName>> unsupportedForTag = new();

    public MarkDownEnvironment(string text)
    {
        markDownTags = new Dictionary<TagName, Tag>
        {
            [TagName.Header] = new Header(this)
        };
    }

    public void AddNewTagForMarkDown(Tag tag)
    {
        throw new NotImplementedException();
    }

    public bool CanGetTagCreator(int start, out Tag tag)
    {
        throw new NotImplementedException();
    }

    public void AddUnsupportedInnersFor(Tag tag, params TagName[] unsupported)
    {
        if (unsupportedForTag.TryGetValue(tag, out var types))
            types.AddRange(unsupported);
        else
            unsupportedForTag[tag] = new List<TagName>(unsupported);
    }

    public IEnumerable<Tag> GetUnsupportedInnersFor(Tag tag)
    {
        return unsupportedForTag.TryGetValue(tag, out var tags) 
            ? tags.Select(e => markDownTags[e])
            : Enumerable.Empty<Tag>();
    }
}