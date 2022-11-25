using System.Text;
using Markdown.Extensions;
using Markdown.Tags;

namespace Markdown.Converter;

public class HtmlConverter : IHtmlConverter
{
    private StringBuilder htmlStringBuilder;
    private int shift;
    private string original;
    private bool hasStartedMarkedList;

    private Dictionary<TagType, string> openingHtmlTags = new()
    {
        { TagType.Bold, "<strong>" }, { TagType.Italic, "<em>" },
        { TagType.Header, "<h1>" }, { TagType.EscapedSymbol, "" },
        { TagType.MarkedListItem, "<li>" }
    };

    private Dictionary<TagType, string> closingHtmlTags = new()
    {
        { TagType.Bold, "</strong>" }, { TagType.Italic, "</em>" },
        { TagType.Header, "</h1>" }, { TagType.EscapedSymbol, "" },
        { TagType.MarkedListItem, "</li>" }
    };

    public IEnumerable<string> ConvertToHtml(string[] lines, List<IEnumerable<Tag>> tags)
    {
        var htmlStrings = new List<string>();
        hasStartedMarkedList = false;
        for (var i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var lineTags = tags[i];
            htmlStringBuilder = new StringBuilder(line);
            var singleTags = lineTags.ConvertToSingleTags();
            shift = 0;
            original = line;
            HandleMarkedList(singleTags);
            foreach (var tag in singleTags)
            {
                ReplaceTag(tag);
            }

            htmlStrings.Add(htmlStringBuilder.ToString());
        }

        if (hasStartedMarkedList)
        {
            htmlStrings.Add("</ul>");
        }

        return htmlStrings;
    }

    private void ReplaceTag(SingleTag tag)
    {
        var symCount = 1;
        if (tag.Type == TagType.Bold)
        {
            symCount = 2;
            shift--;
        }

        if (tag.Type is TagType.Header or TagType.MarkedListItem && tag.IsClosing)
        {
            symCount = 0;
            shift++;
        }

        var tagString = tag.IsClosing ? closingHtmlTags[tag.Type] : openingHtmlTags[tag.Type];
        htmlStringBuilder.Remove(tag.Index + shift, symCount);
        htmlStringBuilder.Insert(tag.Index + shift, tagString);
        shift = htmlStringBuilder.Length - original.Length;
    }

    private void HandleMarkedList(List<SingleTag> singleTags)
    {
        if (singleTags.Count > 0 && singleTags.First().Type == TagType.MarkedListItem && !hasStartedMarkedList)
        {
            hasStartedMarkedList = true;
            htmlStringBuilder.Insert(0, "<ul>\n");
            shift = htmlStringBuilder.Length - original.Length;
        }
        else if ((singleTags.Count == 0 ||
                  (singleTags.Count > 0 && singleTags.First().Type != TagType.MarkedListItem)) && hasStartedMarkedList)
        {
            hasStartedMarkedList = false;
            htmlStringBuilder.Insert(0, "</ul>\n");
            shift = htmlStringBuilder.Length - original.Length;
        }
    }
}