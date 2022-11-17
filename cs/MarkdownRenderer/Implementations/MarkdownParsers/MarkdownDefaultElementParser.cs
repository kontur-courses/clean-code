using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Implementations.MarkdownParsers;

public abstract class MarkdownDefaultElementParser<TElem> : DefaultElementParser
    where TElem : IElement
{
    public override Type ParsingElementType => typeof(TElem);
    public abstract override ElementParseType ParseType { get; }

    public abstract override IElement ParseElement(string content, Token token);
}