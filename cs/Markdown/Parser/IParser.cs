namespace Markdown;

public interface IParser
{
    public IEnumerable<Token> Parse(string text);
}