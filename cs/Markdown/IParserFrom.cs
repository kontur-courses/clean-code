namespace Markdown;

public interface IParserFrom
{
    IEnumerable<MarkupToken> Parse(string text);
}
