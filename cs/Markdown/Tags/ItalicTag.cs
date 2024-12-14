namespace Markdown.Tags;
public class ItalicTag : BaseTagHandler
{
    protected override string Symbol => "_";
    protected override string HtmlTag => "em";

    public override bool IsTagStart(string text, int index) => text[index].ToString() == Symbol;

    protected override int FindEndIndex(string text, int startIndex) =>
    HelperFunctions.FindCorrectCloseSymbolForItalic(text, startIndex + 1);

    protected override string ProcessNestedTag(ref string text) => text;
}