using Markdown.Tags.TagSpecification;

namespace Markdown.Tags;

public record TagReplacementSpecification(Tag Tag, BaseTag Markup, int StartIndex) : IComparable
{
    public int CompareTo(object? obj)
    {
        if (obj is TagReplacementSpecification specification)
            return StartIndex.CompareTo(specification.StartIndex);
        throw new ArgumentException($"Object must be of type {nameof(TagReplacementSpecification)}");
    }
}