using Markdown.Abstractions;
using Markdown.Extensions;
using Markdown.Primitives;

namespace Markdown.Parsers;

public class ItalicParser : UnderlineParser, ITokenParser
{
    public ItalicParser(TokenCollectionParser mainParser) : base(mainParser)
    {
    }

    public TagNode Parse()
    {
        return ParseUnderline(Tokens.Italic);
    }

    protected override bool TryParseNonText(TokenContext context, out TagNode tag)
    {
        var token = Tokens.Italic;
        var children = context.Children
            .Select(x => x.Tag.Type == TagType.Bold ? Tags.Text(x.ToText()).ToTagNode() : x)
            .ToArray();
        tag = new TagNode(token.ToTag(), children);
        return true;
    }
}