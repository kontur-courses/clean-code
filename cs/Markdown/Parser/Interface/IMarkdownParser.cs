namespace Markdown.Parser.Interface;

public interface IMarkdownParser
{
    IEnumerable<Token.Token> Parse(string markdownText);
}
