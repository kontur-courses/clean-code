using Markdown.Tokens;

namespace Markdown.Renderers;

public interface ITokenConverter
{
    public string ConvertTokens(List<Token> tokens);
}