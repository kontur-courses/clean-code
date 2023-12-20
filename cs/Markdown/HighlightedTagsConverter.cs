using System.Text;
using Markdown.Tags;

namespace Markdown;

public class HighlightedTagsConverter(HashSet<ITag> tags)
{
    public string ToHTMLCode(HighlightedData highlighted)
    {
        return ReplaceHighlightedWithHTML(highlighted);
    }

    private string ReplaceHighlightedWithHTML(HighlightedData highlighted)
    {
        var htmlString = new StringBuilder(highlighted.MarkdownText);

        var tagInfoTuples = new List<(ITag tag, bool isCLose, int idx)>();
        foreach (var tag in tags)
        {
            foreach (var pairTagInfo in highlighted.TagsIndexes[tag.GetType()])
            {
                tagInfoTuples.Add((tag, false, pairTagInfo.OpenIdx));
                tagInfoTuples.Add((tag, true, pairTagInfo.CloseIdx));
            }
        }

        foreach (var tagInfo in tagInfoTuples.OrderByDescending(i => i.idx))
        {
            if (tagInfo.tag.GetType() == typeof(HeaderTag) && tagInfo.isCLose)
            {
                htmlString.Insert(tagInfo.idx, tagInfo.tag.Html.Insert(1, "/"));
                continue;
            }

            htmlString.Remove(tagInfo.idx, tagInfo.tag.Md.Length);
            htmlString.Insert(tagInfo.idx, tagInfo.isCLose
                ? tagInfo.tag.Html.Insert(1, "/")
                : tagInfo.tag.Html);
        }

        return htmlString.ToString();
    }
}