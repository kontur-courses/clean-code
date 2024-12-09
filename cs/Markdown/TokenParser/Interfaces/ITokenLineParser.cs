namespace Markdown.TokenParser.Interfaces;

public interface ITokenLineParser
{
    public ParsedLine ParseLine(string text);
}