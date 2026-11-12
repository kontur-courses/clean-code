namespace Markdown
{
    public record OpenedTag(TagType Type, int HtmlPartIndex, bool IsInsideWord, int WordIndex);
}
