using Markdown.Abstractions;
using Markdown.Extensions;
using Markdown.Primitives;

namespace Markdown.Parsers;

public class Header1Parser : ITokenParser
{
    private readonly InnerParser mainParser;

    public Header1Parser(InnerParser mainParser)
    {
        this.mainParser = mainParser;
    }
    
    
    public TagNode Parse()
    {
        if (mainParser.TryMoveNext(out var next))
        {
            mainParser.PushContext(new TokenContext(Tokens.Header1));
            return mainParser.ParseToken(next);
        }

        return Tokens.Header1.ToTagNode();
    }
    
}