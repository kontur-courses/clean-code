using System.Text;
using Markdown.Extensions;
using Markdown.Tags;

namespace Markdown.Converter;

public class HtmlConverter : IHtmlConverter
{
    private StringBuilder htmlStringBuilder;
    private int shift;
    private string original;

    private Dictionary<TagType, string> openingTags = new()
    {
        { TagType.Bold, "<strong>" }, { TagType.Italic, "<em>" },
        { TagType.Header, "<h1>" }, { TagType.EscapedSymbol, "" }
    };

    private Dictionary<TagType, string> closingTags = new()
    {
        { TagType.Bold, "</strong>" }, { TagType.Italic, "</em>" },
        { TagType.Header, "</h1>" }, { TagType.EscapedSymbol, "" }
    };

    public string ConvertToHtml(string original, IEnumerable<Tag> tags)
    {
        htmlStringBuilder = new StringBuilder(original);
        var singleTags = tags.ConvertToSingleTags();
        shift = 0;
        this.original = original;

        foreach (var tag in singleTags)
        {
            ReplaceTag(tag);
        }

        return htmlStringBuilder.ToString();
    }

    private void ReplaceTag(SingleTag tag)
    {
        var symCount = 1;
        if (tag.Type == TagType.Bold)
        {
            symCount = 2;
            shift--;
        }
        if (tag.Type == TagType.Header && tag.IsClosing)
        {
            symCount = 0;
            shift++;
        }
        var tagString = tag.IsClosing ? closingTags[tag.Type] : openingTags[tag.Type];
        htmlStringBuilder.Remove(tag.Index + shift, symCount);
        htmlStringBuilder.Insert(tag.Index + shift, tagString);
        shift = htmlStringBuilder.Length - original.Length;
    }
}