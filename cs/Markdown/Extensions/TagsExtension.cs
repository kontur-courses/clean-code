using Markdown.Tags;

namespace Markdown.Extensions;

public static class TagsExtension
{
    public static List<SingleTag> ConvertToSingleTags(this IEnumerable<Tag> tags)
    {
        var singleTags = new List<SingleTag>();
        foreach (var tag in tags)
        {
            singleTags.Add(new SingleTag(tag.Type,false,tag.Position.Start));
            if (tag.Type != TagType.EscapedSymbol)
            {
                singleTags.Add(new SingleTag(tag.Type, true, tag.Position.End));
            }
        }
        return singleTags.OrderBy(tag => tag.Index).ThenBy(tag => tag.Type).ToList();
    }
}