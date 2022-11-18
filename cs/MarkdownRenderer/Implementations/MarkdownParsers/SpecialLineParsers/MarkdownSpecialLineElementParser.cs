using MarkdownRenderer.Abstractions.Elements;
using MarkdownRenderer.Abstractions.ElementsParsers;

namespace MarkdownRenderer.Implementations.MarkdownParsers.SpecialLineParsers;

public abstract class MarkdownSpecialLineElementParser<TElem> : ISpecialLineElementParser
    where TElem : IElement
{
    public Type ParsingElementType { get; } = typeof(TElem);

    public abstract string Prefix { get; }

    public abstract string Postfix { get; }

    protected abstract Func<string, TElem> ElementCreator { get; }

    public virtual bool Match(string line) =>
        line.StartsWith(Prefix) && line.EndsWith(Postfix);

    IElement ILineElementParser.ParseElement(string content) =>
        ParseElement(content);

    public TElem ParseElement(string content)
    {
        if (!TryParseElement(content, out var element))
            throw new ArgumentException("Unable to parse!");
        return element!;
    }

    bool ISpecialLineElementParser.TryParseElement(string content, out IElement? element)
    {
        var result = TryParseElement(content, out var tElement);
        element = tElement;
        return result;
    }

    public virtual bool TryParseElement(string content, out TElem? element)
    {
        element = default;
        if (!Match(content))
            return false;

        var rawContent = content.Substring(Prefix.Length, content.Length - (Prefix.Length + Postfix.Length));

        element = ElementCreator(rawContent);
        return true;
    }
}