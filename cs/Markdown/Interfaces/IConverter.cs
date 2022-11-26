using Markdown.Tokens;

namespace Markdown.Interfaces;

public interface IConverter
{
    public string ConvertTokens(List<Token> tokens);
}