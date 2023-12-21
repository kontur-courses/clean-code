using Markdown.Tokens.Decorators;

namespace Markdown.Filter;

public class TokenFilterChain
{
    private TokenFilterChain? nextHandler;
    
    public TokenFilterChain SetNext(TokenFilterChain handler)
    {
        nextHandler = handler;
        return handler;
    }

    public virtual List<TokenFilteringDecorator> Handle(List<TokenFilteringDecorator> tokens, string line)
        => nextHandler?.Handle(tokens, line) ?? tokens;
}