using Markdown.Tokens;

namespace Markdown.Filter;

public class TokenFilterChain
{
    private TokenFilterChain? nextHandler;

    public TokenFilterChain SetNext(TokenFilterChain handler)
    {
        nextHandler = handler;
        return handler;
    }

    public virtual List<Token> Handle(List<Token> tokens, string line)
        => nextHandler?.Handle(tokens, line) ?? tokens;
}