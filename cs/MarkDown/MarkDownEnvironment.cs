using MarkDown.Enums;
using MarkDown.Tags;
using MarkDown.Tags.Abstracts;

namespace MarkDown;

public class MarkDownEnvironment
{
    private readonly Dictionary<TagName, TagFactory> markDownTags = new();
    // private readonly Dictionary<TagName, List<TagName>> unsupportedForTag = new();
    private readonly Dictionary<TagFactory, List<TagName>> unsupportedParentsForTag = new();

    public MarkDownEnvironment()
    {
        AddNewTagForMarkDown(new HeaderTagFactory(this));
        AddNewTagForMarkDown(new EntryTagFactory(this));
        AddNewTagForMarkDown(new StrongTagFactory(this));
        AddNewTagForMarkDown(new EmTagFactory(this));
    }

    private void AddNewTagForMarkDown(TagFactory tagFactory)
    {
        markDownTags[tagFactory.TagName] = tagFactory;
    }

    public bool CanGetTagCreator(string text, int position, out TagFactory openTagFactory)
    {
        openTagFactory = null;
        
        foreach (var tag in markDownTags.Values.Where(tag => tag.CanCreateContext(text, position)))
            if (openTagFactory != null)
            {
                if (openTagFactory.MarkDownOpen.Length < tag.MarkDownOpen.Length)
                    openTagFactory = tag;
            }
            else
                openTagFactory = tag;

        return openTagFactory is not null;
    }

    public bool CanGetCloseTags(string text, int position, out List<TagFactory> closeTags)
    {
        closeTags = new List<TagFactory>();
        
        foreach (var tag in markDownTags.Values)
            if (tag.IsClosePosition(text, position))
                closeTags.Add(tag);

        return closeTags.Count > 0;
    }

    public TagFactory GetTagByName(TagName tagName)
    {
        return markDownTags[tagName];
    }

    // public void AddUnsupportedInnersFor(TagName tagName, params TagName[] unsupported)
    // {
    //     if (unsupportedForTag.TryGetValue(tagName, out var types))
    //         types.AddRange(unsupported);
    //     else
    //         unsupportedForTag[tagName] = new List<TagName>(unsupported);
    // }

    public void AddUnsupportedParentsFor(TagFactory tagFactory, params TagName[] unsupported)
    {
        if (unsupportedParentsForTag.TryGetValue(tagFactory, out var tags))
            tags.AddRange(unsupported);
        else
            unsupportedParentsForTag[tagFactory] = new List<TagName>(unsupported);
    }
    
    public IEnumerable<TagName> GetUnsupportedParentsFor(TagFactory tagFactory)
    {
        return unsupportedParentsForTag.TryGetValue(tagFactory, out var tags) 
            ? tags
            : Enumerable.Empty<TagName>();
    }

    // public IEnumerable<TagName> GetUnsupportedInnersFor(TagName tagName)
    // {
    //     return unsupportedForTag.TryGetValue(tagName, out var tags) 
    //         ? tags
    //         : Enumerable.Empty<TagName>();
    // }
}