namespace Markdown.Converter;

public interface IConverter
{
    public string Convert(ParsedLine[] parsedLines);
}