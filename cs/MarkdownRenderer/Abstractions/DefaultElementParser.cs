namespace MarkdownRenderer.Abstractions;

public abstract class DefaultElementParser : IElementParser
{
    public abstract Type ParsingElementType { get; }
    public abstract ElementParseType ParseType { get; }
    string IElementParser.Prefix => "";
    string IElementParser.Postfix => "";

    bool IElementParser.IsElementStart(string content, int index)
    {
        throw new InvalidOperationException("Default parser has no specific start.");
    }

    bool IElementParser.IsElementEnd(string content, int index)
    {
        throw new InvalidOperationException("Default parser has no specific end.");
    }

    bool IElementParser.TryParseElement(string source, Token token, out IElement? element)
    {
        element = ParseElement(source, token);
        return true;
    }

    public abstract IElement ParseElement(string content, Token token);
}