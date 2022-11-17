using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.MarkdownParsers;

public class MarkdownItalicParser : MarkdownInlineParser<ItalicElement>
{
    public override string Prefix => "_";
    public override string Postfix => "_";
    protected override Func<string, ItalicElement> ElementCreator { get; } = content => new ItalicElement(content);
}