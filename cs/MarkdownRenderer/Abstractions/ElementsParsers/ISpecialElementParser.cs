namespace MarkdownRenderer.Abstractions.ElementsParsers;

public interface ISpecialElementParser : IElementParser
{
    string Prefix { get; }
    string Postfix { get; }
}