using MarkdownRenderer.Implementations.Elements;

namespace MarkdownRenderer.Implementations.MarkdownParsers;

public class MarkdownHeaderParser : MarkdownSpecialLineElementParser<HeaderElement>
{
    public override string Prefix => "# ";
    public override string Postfix => "";
    protected override Func<string, HeaderElement> ElementCreator { get; } = content => new HeaderElement(content);
}