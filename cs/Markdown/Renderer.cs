using System.Text;

namespace Markdown;

public class Renderer
{
    public string RenderTags(IEnumerable<MarkdownTagInfo> renderTags, string markdownText)
    {
        var tagsToRender = renderTags.ToArray();
        var stringBuilder = new StringBuilder();
        var tagIndex = 0;
        var tagInfo = tagsToRender[tagIndex];
        //for (var i = 0; i < markdownText.Length; i++)
        //{
        //    if (i != tagInfo.StartIndex)
        //    {
        //        stringBuilder.Append(markdownText[i]);
        //    }
        //    else
        //    {
        //        if (tagInfo.Tag != Tags.Escape)
        //        {
        //            stringBuilder.Append(tagInfo.IsOpening ? tagInfo.Tag.TagOpen : tagInfo.Tag.TagClose);
        //            i = tagInfo.EndIndex;
        //        }
        //        tagIndex++;
        //        if (tagIndex < tagsToRender.Length)
        //            tagInfo = tagsToRender[tagIndex];
        //    }
        //}

        return stringBuilder.ToString();
    }
}