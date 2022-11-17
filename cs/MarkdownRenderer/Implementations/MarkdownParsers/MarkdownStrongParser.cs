using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.MarkdownParsers;

public class MarkdownStrongParser : MarkdownInlineParser<StrongElement>
{
    public override string Prefix => "__";
    public override string Postfix => "__";
    protected override Func<string, StrongElement> ElementCreator { get; } = content => new StrongElement(content);
}