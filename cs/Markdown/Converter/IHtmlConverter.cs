using Markdown.Tags;

namespace Markdown.Converter;

public interface IHtmlConverter
{
    public string ConvertToHtml(string original, IEnumerable<Tag> tags);
}