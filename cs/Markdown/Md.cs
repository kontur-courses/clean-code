using System.Text;

namespace Markdown;

public class Md
{
    public static string Render(string textInMd)
    {
        var renderedText = new StringBuilder();
        foreach (var paragraph in GetParagraphs(textInMd))
        {
            renderedText.Append(RenderParagraph(paragraph));
        }

        return renderedText.ToString();
    }

    private static string RenderParagraph(string paragraph)
    {
        var tags = GetTags(paragraph);
        var textWithoutTags = DeleteTags(paragraph, tags);
        return InsertHtmlTags(textWithoutTags, tags);
    }

    private static Tag[] GetTags(string text)
    {
        throw new NotImplementedException();
    }

    private static bool TagIsOk(Tag tag)
    {
        throw new NotImplementedException();
    }

    private static string DeleteTags(string text, Tag[] tags)
    {
        throw new NotImplementedException();
    }

    private static IEnumerable<string> GetParagraphs(string text)
    {
        throw new NotImplementedException();
    }

    private static string InsertHtmlTags(string text, Tag[] tags)
    {
        throw new NotImplementedException();
    }
}