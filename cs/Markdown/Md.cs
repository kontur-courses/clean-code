using Markdown.Parse;
using Markdown.Render;
using Markdown.Tags;

namespace Markdown;

public class Md : IMarkupRenderer
{
    private readonly Dictionary<Tag, Tag> renderingRules = new Dictionary<Tag, Tag>
    {
        { MarkdownTags.Bold, HtmlTags.Bold },
        { MarkdownTags.Italics, HtmlTags.Italics },
        { MarkdownTags.Heading, HtmlTags.Heading },
    };
    
    public string Render(string text)
    {
        var renderer = new TokenRenderer();
        var parser = new MarkdownMarkupParser();

        return renderer.Render(parser.Parse(text), renderingRules);
    }
}