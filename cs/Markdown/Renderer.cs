using System.Text;
using Markdown.TagClasses;

namespace Markdown;

public class Renderer
{
    public string RenderHtmlTags(IEnumerable<MarkdownTagInfo> renderTags, string markdownText)
    {
        var tagsToRender = renderTags.ToArray();
        var stringBuilder = new StringBuilder();
        var tagIndex = 0;
        var tagInfo = tagsToRender.Length > 0 ? tagsToRender[tagIndex] : new MarkdownTagInfo(null, -1, -1);
        for (var i = 0; i < markdownText.Length; i++)
        {
            if (i != tagInfo.StartIndex)
            {
                stringBuilder.Append(markdownText[i]);
            }
            else
            {
                stringBuilder.Append(tagInfo.IsOpening ? tagInfo.Tag.HtmlTagOpen : tagInfo.Tag.HtmlTagClose);
                i = tagInfo.EndIndex;
                if (++tagIndex < tagsToRender.Length)
                    tagInfo = tagsToRender[tagIndex];
            }
        }

        return stringBuilder.ToString();
    }
}