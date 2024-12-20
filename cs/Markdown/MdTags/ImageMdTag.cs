using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags;

internal class ImageMdTag : IMdTag, IHtmlConverter
{
    private const string MdTag = "!";

    private readonly LinkMdTag linkMdTag = new();

    public TagType TagType => TagType.Normal;

    public bool IsMdTag(string mdString, int startIndex, out int tagLength)
    {
        if (IsImageTag(mdString, startIndex, out var titleLength, out var pathLength))
        {
            tagLength = MdTag.Length + titleLength + pathLength;
            return true;
        }

        tagLength = default;
        return false;
    }

    public string GetHtmlString(string mdString, int startIndex)
    {
        if (IsImageTag(mdString, startIndex, out var titleLegnth, out var pathLength))
        {
            var titleStartIndex = startIndex + MdTag.Length;
            var titleEndIndex = titleStartIndex + titleLegnth;
            var pathEndIndex = titleEndIndex + pathLength;
            var title = mdString[(titleStartIndex + 1)..(titleEndIndex - 1)];
            var path = mdString[(titleEndIndex + 1)..(pathEndIndex - 1)];
            return $"<img src=\"{path}\" alt=\"{title}\">";
        }

        throw new InvalidOperationException($"There is no image tag at {startIndex} in '{mdString}'.");
    }

    private bool IsImageTag(string mdString, int startIndex, out int titleLength, out int pathLength)
    {
        titleLength = default;
        pathLength = default;
        return TagSearchHelper.IsSubstring(mdString, MdTag, startIndex)
            && linkMdTag.IsLinkTagWithTitle(mdString, startIndex + MdTag.Length, out titleLength, out pathLength);
    }
}
