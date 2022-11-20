using Markdown.Tags;

namespace Markdown.Converter;

public interface IMarkupConverter
{
    public string ConvertToMyMarkup(string original, IEnumerable<Tag> tags);
}