namespace MarkdownRenderer.Abstractions;

public interface IInlineElementParser : IElementParser
{
    IElement ParseElement(string content, Token token);
}