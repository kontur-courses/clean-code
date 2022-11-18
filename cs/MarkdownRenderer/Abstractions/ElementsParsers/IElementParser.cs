namespace MarkdownRenderer.Abstractions.ElementsParsers;

public interface IElementParser
{
    Type ParsingElementType { get; }
}