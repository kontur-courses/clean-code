using Markdown.MdTags.Interfaces;

namespace Markdown.MdTags;

internal class LinkMdTag : IMdTag, IHtmlConverter
{
    public TagType TagType => TagType.Normal;

    public bool IsMdTag(string mdString, int startIndex, out int tagLength)
    {
        if (IsAngleBracketsLinkTag(mdString, startIndex, out tagLength))
        {
            return true;
        }

        if (IsLinkTagWithTitle(mdString, startIndex, out var titleLength, out var linkLength))
        {
            tagLength = titleLength + linkLength;
            return true;
        }

        tagLength = default;
        return false;
    }

    public string GetHtmlString(string mdString, int startIndex)
    {
        if (IsAngleBracketsLinkTag(mdString, startIndex, out var tagLength))
        {
            var link = mdString[(startIndex + 1)..(startIndex + tagLength - 1)];
            return $"<a href=\"{link}\">{link}</a>";
        }

        if (IsLinkTagWithTitle(mdString, startIndex, out var titleLegnth, out var linkLength))
        {
            var titleEndIndex = startIndex + titleLegnth;
            var linkEndIndex = titleEndIndex + linkLength;
            var title = mdString[(startIndex + 1)..(titleEndIndex - 1)];
            var link = mdString[(titleEndIndex + 1)..(linkEndIndex - 1)];
            return $"<a href=\"{link}\">{title}</a>";
        }

        throw new InvalidOperationException($"There is no link tag at {startIndex} in '{mdString}'.");
    }

    public bool IsLinkTagWithTitle(string mdString, int startIndex, out int titleLength, out int linkLength)
    {
        linkLength = default;
        return TagSearchHelper.IsParenthesis(mdString, '[', ']', startIndex, out titleLength)
            && TagSearchHelper.IsParenthesis(mdString, '(', ')', startIndex + titleLength, out linkLength);
    }

    private bool IsAngleBracketsLinkTag(string mdString, int startIndex, out int tagLength)
    {
        return TagSearchHelper.IsParenthesis(mdString, '<', '>', startIndex, out tagLength);
    }
}
