using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.MarkdownParsers.SpecialInlineParsers;

public class MarkdownStrongParser : MarkdownSpecialInlineElementParser<StrongElement>
{
    public override string Prefix => "__";
    public override string Postfix => "__";

    public override bool IsElementEnd(string content, int index)
    {
        if (!base.IsElementEnd(content, index))
            return false;

        return content[index - Postfix.Length] is not '_' &&
               (index + 1 >= content.Length || content[index + 1] is not '_');
    }

    protected override Func<string, StrongElement> ElementCreator { get; } = content => new StrongElement(content);
}