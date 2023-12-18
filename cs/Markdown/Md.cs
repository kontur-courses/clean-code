using Markdown.Tags;

namespace Markdown;

public class Md
{
    private readonly List<ITag> tags;

    public Md()
    {
        tags = new List<ITag>
        {
            new EmTag(),
            new StrongTag(),
            new HeaderTag()
        };
    }

    public Md(params ITag[] tags)
    {
        this.tags = new List<ITag>(tags);
    }

    // _some string_ => <em>some string</em>
    public string Render(string markdownText)
    {
        var highlighted = new TagsHighlighter(tags).HighlightMdTags(markdownText);

        highlighted = highlighted.ReplaceShieldSequence();

        return new HighlightedTagsConverter().ToHTMLCode(highlighted);
    }
}