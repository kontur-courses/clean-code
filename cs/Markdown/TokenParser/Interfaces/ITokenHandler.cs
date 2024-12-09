using Markdown.Tokens;

namespace Markdown.TokenParser.Interfaces;

public interface ITokenHandler
{
    List<Token> HandleLine(List<Token> line);
}