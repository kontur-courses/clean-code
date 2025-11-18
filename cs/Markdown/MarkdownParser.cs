using Markdown.TokenHandlers;
using Markdown.Tokens;

namespace Markdown;

internal class MarkdownParser
{
    private readonly IEnumerable<ITokenHandler> _handlers;
    private readonly NestingHandler _nestingHandler;

    public MarkdownParser(IEnumerable<ITokenHandler> handlers, NestingHandler nestingHandler)
    {
        _handlers = handlers;
        _nestingHandler = nestingHandler;
    }

    public IEnumerable<IToken> Parse(IEnumerable<IToken> tokens)
    {
        var tokenList = new LinkedList<IToken>(tokens);
        var stack = new Stack<TagToken>();
        
        for (var node = tokenList.First; node != null; node = node.Next)
        {
            foreach (var handler in _handlers)
                handler.Handle(node, stack);
        }
        
        _nestingHandler.HandleNesting(tokenList);

        return tokenList;
    }
}