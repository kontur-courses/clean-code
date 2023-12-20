using MarkDown.Enums;
using MarkDown.Tags;

namespace MarkDown;

public class MarkDownEnvironment
{
    private readonly Dictionary<TagName, Tag> markDownTags = new();
    private readonly Dictionary<TagName, List<TagName>> unsupportedForTag = new();

    public MarkDownEnvironment()
    {
        AddNewTagForMarkDown(new HeaderTag(this));
        AddNewTagForMarkDown(new EntryTag(this));
    }

    public void AddNewTagForMarkDown(Tag tag)
    {
        markDownTags[tag.TagName] = tag;
    }

    public bool CanGetTagCreator(string text, int position, out Tag openTag)
    {
        openTag = null;
        
        foreach (var tag in markDownTags.Values)
            if (tag.CanCreateContext(text, position))
                openTag = tag;

        return openTag is not null;
    }

    public bool CanGetCloseTags(string text, int position, out List<Tag> closeTags)
    {
        closeTags = new List<Tag>();
        
        foreach (var tag in markDownTags.Values)
            if (tag.IsClosePosition(text, position))
                closeTags.Add(tag);

        return closeTags.Count > 0;
    }

    public Tag GetTagByName(TagName tagName)
    {
        return markDownTags[tagName];
    }

    public void AddUnsupportedInnersFor(TagName tagName, params TagName[] unsupported)
    {
        if (unsupportedForTag.TryGetValue(tagName, out var types))
            types.AddRange(unsupported);
        else
            unsupportedForTag[tagName] = new List<TagName>(unsupported);
    }

    public IEnumerable<TagName> GetUnsupportedInnersFor(TagName tagName)
    {
        return unsupportedForTag.TryGetValue(tagName, out var tags) 
            ? tags
            : Enumerable.Empty<TagName>();
    }
}