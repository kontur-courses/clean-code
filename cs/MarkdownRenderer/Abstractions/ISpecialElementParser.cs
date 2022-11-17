namespace MarkdownRenderer.Abstractions;

public interface ISpecialElementParser : IElementParser
{
    string Prefix { get; }
    string Postfix { get; }
}