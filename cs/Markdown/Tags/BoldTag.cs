namespace Markdown.Tags;
public class BoldTag : BaseTagHandler
{
    protected override string Symbol => "__";
    protected override string HtmlTag => "strong";

    public override bool IsTagStart(string text, int index) => index + 2 <= text.Length
        && text.Substring(index, 2) == Symbol;
}