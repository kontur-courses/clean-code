using Markdown.Tokens;

namespace Markdown;

public interface IParser
{
    List<Token> Parse(string text);
}