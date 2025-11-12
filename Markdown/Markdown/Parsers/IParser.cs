using Markdown.Tokens;

namespace Markdown.Parsers;

public interface IParser
{
    public bool CanParse(char symbol, string text, int index);
    public Token Parse(string text, int index);
}