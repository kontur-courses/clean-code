using Markdown.Abstractions;
using Markdown.Extensions;
using Markdown.Primitives;

namespace Markdown.Parsers;

public class TextParser : ITokenParser
{
    private readonly TokenCollectionParser mainParser;

    public TextParser(TokenCollectionParser mainParser)
    {
        this.mainParser = mainParser;
    }

    public TagNode Parse() => mainParser.Current.ToTagNode();
}