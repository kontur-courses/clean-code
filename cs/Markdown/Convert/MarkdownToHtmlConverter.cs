using Markdown.Tags;
using Markdown.Tags.HtmlTags;
using Markdown.Tags.MarkdownTags;

namespace Markdown.Convert;

public class MarkdownToHtmlConverter : ITagConverter
{
    private readonly Dictionary<Tag, Tag> conversionRules= new() 
    {
        { MarkdownTagProvider.Bold, HtmlTagProvider.Bold },
        { MarkdownTagProvider.Italics, HtmlTagProvider.Italics },
        { MarkdownTagProvider.Heading, HtmlTagProvider.Heading },
    };

    public Tag Convert(Tag tag)
    {
        if (!conversionRules.ContainsKey(tag))
            throw new NotSupportedException();

        return conversionRules[tag];
    }
}