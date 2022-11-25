using Markdown.Parse;
using Markdown.Render;
using Markdown.Tags;
using Markdown.Tags.HtmlTags;
using Markdown.Tags.MarkdownTags;

namespace Markdown;

public class Md : IMarkupRenderer
{
    private readonly Dictionary<Tag, Tag> renderingRules = new Dictionary<Tag, Tag>
    {
        { MarkdownTagProvider.Bold, HtmlTagProvider.Bold },
        { MarkdownTagProvider.Italics, HtmlTagProvider.Italics },
        { MarkdownTagProvider.Heading, HtmlTagProvider.Heading },
    };
    
    public string Render(string text)
    {
        var renderer = new TokenRenderer();
        var parser = new MarkdownMarkupParser();

        return renderer.Render(parser.Parse(text), renderingRules);
    }
}