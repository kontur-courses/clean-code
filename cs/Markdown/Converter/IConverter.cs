namespace Markdown.Converter;

public interface IConverter
{
    public string Convert(string original, IEnumerable<TagPosition> tagsSpots);
}