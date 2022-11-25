using Markdown.Tags;

namespace Markdown.Converter;

public interface IHtmlConverter
{
    public IEnumerable<string> ConvertToHtml(string[] original, List<IEnumerable<Tag>> tags);
}