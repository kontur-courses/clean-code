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

        foreach (var tag in tags)
        {
            if (tag.GetType().GetInterface(nameof(ISingleTag)) != null)
            {
                IEnumerable<int> indexes = highlighted.SingleTagsIndexes[tag.GetType()];
                foreach (var idx in indexes.Reverse())
                {
                    htmlString.Remove(idx, tag.Md.Length);
                    htmlString.Insert(idx, tag.Html);

                    if (tag.GetType() == typeof(HeaderTag))
                    {
                        var closeIndexOfParagraph = htmlString.ToString().CloseIndexOfParagraph(idx);
                        htmlString.Insert(closeIndexOfParagraph, tag.Html.Insert(1, "/"));
                    }
                }
            }
            else
            {
                IEnumerable<PairTagInfo> indexes = highlighted.PairTagsIndexes[tag.GetType()];
                foreach (var pairTagInfo in indexes.Reverse())
                {
                    htmlString.Remove(pairTagInfo.CloseIdx, tag.Md.Length);
                    htmlString.Insert(pairTagInfo.CloseIdx, tag.Html.Insert(1, "/"));
                    
                    htmlString.Remove(pairTagInfo.OpenIdx, tag.Md.Length);
                    htmlString.Insert(pairTagInfo.OpenIdx, tag.Html);
                }
            }
        }

        return htmlString.ToString();
    }
}