using Markdown.Tags;

namespace Markdown;

public class Md
{
    private readonly HashSet<ITag> tags;

    public Md()
    {
        tags = new HashSet<ITag>
        {
            new EmTag(),
            new StrongTag(),
            new HeaderTag()
        };
    }

    public Md(params ITag[] tags)
    {
        this.tags = new HashSet<ITag>(tags);
    }

    public string Render(string markdownText)
    {
        var highlighted = new TagsHighlighter(tags).HighlightMdTags(markdownText);
        
        var converted = new HighlightedTagsConverter(tags).ToHTMLCode(highlighted);

        return converted.ReplaceShieldSequences();
    }
}