using Markdown.Models;

namespace Markdown.Converters.Base;

public interface IConverter
{
    public IEnumerable<TagPair> Convert(string text);
}