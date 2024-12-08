using Markdown.Token;

namespace Markdown.Parser.Interface;

public interface ITokenHandler
{
    bool TryHandle(ParsingContext context, out Token.Token token, out int skip);
}