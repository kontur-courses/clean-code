using Markdown.Markdown.Tokens;

namespace Markdown.Markdown.Handlers;

public interface ITokenHandler
{
    IReadOnlyList<IToken> Handle(IReadOnlyList<IToken> tokens);
}