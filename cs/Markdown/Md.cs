using System.Collections.ObjectModel;

namespace Markdown;

public class Md
{
    private readonly Dictionary<string, string> mdToHtmlConversion = new()
    {
        { "_", "<em>" },
        { "__", "<strong>" },
        { "#", "\\<h1>" }
    };

    public ReadOnlyDictionary<string, string> MToHtmlConversion => mdToHtmlConversion.AsReadOnly();

    public Md(string[] tags)
    {
        foreach (var tag in mdToHtmlConversion.Keys)
            if (!tags.Contains(tag))
                mdToHtmlConversion.Remove(tag);
    }

    // _some string_ => <em>some string</em>
    public string Render(string markdownText)
    {
        return new TagTextConverter().ToHTMLCode(markdownText);
    }
}