using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.MarkdownParsers.SpecialLineParsers;

public class MarkdownHeaderParser : MarkdownSpecialLineElementParser<HeaderElement>
{
    public override string Prefix => "# ";
    public override string Postfix => "";
    protected override Func<HeaderElement> ElementCreator { get; } = () => new HeaderElement();
}