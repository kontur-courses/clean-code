using Markdown.Converters;
using System.Text;

namespace Markdown;

public static class Md
{
    public static string Render(string text)
    {
        var renderedText = new StringBuilder();
        var paragraphs = text.Split("\n");
        for (var i = 0; i < paragraphs.Length; i++)
        {
            renderedText.Append(RenderParagraph(paragraphs[i]));
            if (i < paragraphs.Length - 1) renderedText.Append('\n');
        }  
        return renderedText.ToString();
    }

    private static StringBuilder RenderParagraph(string paragraph)
    {
        var tagParser = new TagsParser(paragraph);
        var tags = tagParser.BuildTags();
        return MarkdownToHtmlConverter.Convert(tags);
    }
}
