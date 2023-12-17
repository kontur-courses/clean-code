using System.Collections.ObjectModel;
using Markdown.Tags;

namespace Markdown;

public class Md
{
    private readonly List<ITag> mdToHtmlConversion = new();

    public ReadOnlyCollection<ITag> MToHtmlConversion => mdToHtmlConversion.AsReadOnly();

    public Md()
    {
        mdToHtmlConversion.Add(new EmTag());
        mdToHtmlConversion.Add(new StrongTag());
        mdToHtmlConversion.Add(new HeaderTag());
    }

    public Md(params Tag[] tags)
    {
        foreach (var tag in tags)
            switch (tag)
            {
                case Tag.EmTag:
                    mdToHtmlConversion.Add(new EmTag());
                    break;
                case Tag.StrongTag:
                    mdToHtmlConversion.Add(new StrongTag());
                    break;
                case Tag.HeaderTag:
                    mdToHtmlConversion.Add(new HeaderTag());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
    }

    // _some string_ => <em>some string</em>
    public string Render(string markdownText)
    {
        var highlighted = new TagsHighlighter().HighlightMdTags(markdownText);

        return new HighlightedTagsConverter().ToHTMLCode(highlighted);
    }
}