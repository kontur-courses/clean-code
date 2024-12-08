namespace Markdown;

public interface IMarkdownParser
{
    public List<Token> ParseTextToTokens(string text);
}
