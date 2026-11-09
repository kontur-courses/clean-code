namespace Markdown;

public interface IParserTo
{
    string Parse(IEnumerable<MarkupToken> tokens);
}