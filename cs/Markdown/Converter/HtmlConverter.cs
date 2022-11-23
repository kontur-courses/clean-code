using System.Text;
using Markdown.Tags;

namespace Markdown.Converter;

public class HtmlConverter : IHtmlConverter
{
    private StringBuilder htmlStringBuilder;
    public string ConvertToHtml(string original, IEnumerable<Tag> tags)
    {
        htmlStringBuilder = new StringBuilder(original);
        tags = tags.OrderByDescending(tag => tag.Position.End);
        var shift = 0;

        foreach (var tag in tags)
        {
            switch (tag.Type)
            {
                case TagType.EscapedSymbol:
                    htmlStringBuilder.Remove(tag.Position.Start + shift, 1);
                    shift--;
                    break;
                case TagType.Italic:
                    htmlStringBuilder.Remove(tag.Position.Start + shift, 1);
                    htmlStringBuilder.Insert(tag.Position.Start + shift, "<em>");
                    shift += 3;
                    htmlStringBuilder.Remove(tag.Position.End + shift, 1);
                    htmlStringBuilder.Insert(tag.Position.End + shift, "</em>");
                    break;
                case TagType.Bold:
                    shift--;
                    htmlStringBuilder.Remove(tag.Position.Start + shift, 2);
                    htmlStringBuilder.Insert(tag.Position.Start + shift, "<strong>");
                    shift += 6;
                    htmlStringBuilder.Remove(tag.Position.End + shift, 2);
                    htmlStringBuilder.Insert(tag.Position.End  + shift, "</strong>");
                    break;
                case TagType.Header:
                    htmlStringBuilder.Remove(tag.Position.Start + shift, 1);
                    htmlStringBuilder.Insert(tag.Position.Start + shift, "<h1>");
                    shift += 3;
                    htmlStringBuilder.Remove(tag.Position.End + shift, 1);
                    htmlStringBuilder.Insert(tag.Position.End + shift, "</h1>");
                    break;
            }
        }

        return htmlStringBuilder.ToString();
    }
}