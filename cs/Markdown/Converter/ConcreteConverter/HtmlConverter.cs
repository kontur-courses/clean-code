using System.Text;
using Markdown.Tags.ConcreteTags;

namespace Markdown.Converter.ConcreteConverter;

public class HtmlConverter : IConverter
{
    public string Convert(ParsedLine[] parsedLines)
    {
        var sb = new StringBuilder();

        var containList = false;
        var startedLine = true;
        foreach (var text in parsedLines)
        {
            if (!startedLine)
                sb.Append('\n');

            if (containList && text.Tags.FirstOrDefault() is not BulletTag)
            {
                containList = false;
                sb.Append("</ul>");
            }
            else if (!containList && text.Tags.FirstOrDefault() is BulletTag)
            {
                containList = true;
                sb.Append("<ul>");
            }

            var prevTagPos = 0;
            foreach (var tag in text.Tags)
            {
                sb.Append(text.Line.AsSpan(prevTagPos, tag.Position - prevTagPos));

                sb.Append(tag.IsCloseTag ? 
                    MdTagToHtmlConverter.CloseTags[tag.TagType] : 
                    MdTagToHtmlConverter.OpenTags[tag.TagType]);

                prevTagPos = tag.Position;
            }

            sb.Append(text.Line.AsSpan(prevTagPos, text.Line.Length - prevTagPos));
            startedLine = false;
        }

        if (containList)
            sb.Append("</ul>");

        return sb.ToString();
    }
}