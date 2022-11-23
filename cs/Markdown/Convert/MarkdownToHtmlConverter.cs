using Markdown.Parse;
using Markdown.Render;
using Markdown.Tags;

namespace Markdown.Convert;

public class MarkdownToHtmlConverter : IConverter
{
    private readonly Dictionary<Tag, Tag> conversionRules = new Dictionary<Tag, Tag>
    { 
        { MarkdownTags.Bold, HtmlTags.Bold },
        { MarkdownTags.Italics, HtmlTags.Italics },
        { MarkdownTags.Heading, HtmlTags.Heading },
    };

    public string Convert(string text)
    {
        var renderer = new Renderer();
        var parser = new MarkdownParser();

        return renderer.Render(parser.Parse(text), conversionRules);
    }
}