using Markdown.Tokens;

namespace Markdown.TokenParsers.TokenHandlers;

public interface ITokenHandler
{
    public int Priority { get; }

    public IReadOnlyList<Token> Handle(IReadOnlyList<Token> tokens);
}