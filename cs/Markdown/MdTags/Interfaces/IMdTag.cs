namespace Markdown.MdTags.Interfaces;

internal interface IMdTag
{
    public TagType TagType { get; }

    public bool IsMdTag(string mdString, int startIndex, out int tagLength);
}
