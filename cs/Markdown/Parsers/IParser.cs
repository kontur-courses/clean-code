namespace Markdown.Parsers;

public interface IParser
{
    bool TryParse(char symbol, string text, int index);
    Token Parse(string text, ref int index);
}