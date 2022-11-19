using MarkdownProcessor.Markdown;

namespace MarkdownProcessor;

public class HtmlRenderer
{
    public static Dictionary<TextType, string> tagNames = new()
    {
        { TextType.Italic, "em" },
        { TextType.Bold, "strong" },
        { TextType.FirstHeader, "h1" }
    };

    public static string Render(string text, IMarkdownTag markdownTagTree)
    {
        throw new NotImplementedException();
    }
}