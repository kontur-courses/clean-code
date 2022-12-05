using Markdown.Abstractions;
using Markdown.Primitives;

namespace Markdown.Parsers;

public class BoldParser : UnderlineParser, ITokenParser
{
    public BoldParser(InnerParser mainParser) : base(mainParser)
    {
    }
    
    public TagNode Parse() => ParseUnderline(Tokens.Bold);

    protected override bool TryParseNonText(TokenContext context, out TagNode tag)
    {
        tag = null!;
        return false;
    }
}