using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.MarkdownParsers.SpecialInlineParsers;

public class MarkdownItalicParser : MarkdownSpecialInlineElementParser<ItalicElement>
{
    public override string Prefix => "_";
    public override string Postfix => "_";

    public override bool IsElementStart(string content, int index)
    {
        if (!base.IsElementStart(content, index))
            return false;
        return content[index + Prefix.Length] is not '_' &&
               (index == 0 || content[index - 1] is not '_');
    }

    public override bool IsElementEnd(string content, int index)
    {
        if (!base.IsElementEnd(content, index))
            return false;

        return content[index - Postfix.Length] is not '_' &&
               (index + 1 >= content.Length || content[index + 1] is not '_');
    }

    protected override Func<string, ItalicElement> ElementCreator { get; } = content => new ItalicElement(content);
}