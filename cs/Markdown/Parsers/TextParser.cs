using Markdown.Abstractions;
using Markdown.Extensions;
using Markdown.Primitives;

namespace Markdown.Parsers;

public class TextParser : ITokenParser
{
    private readonly InnerParser mainParser;

    public TextParser(InnerParser mainParser)
    {
        this.mainParser = mainParser;
    }

    public TagNode Parse() => mainParser.Current.ToTagNode();
}