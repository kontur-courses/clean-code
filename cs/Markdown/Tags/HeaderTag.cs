using Markdown.Tokens;

namespace Markdown.Tags;

public class HeaderTag : Tag
{
    public override string MdOpen => "# ";
    public override string MdClose => "\n";
    public override string HtmlOpen => "<h1>";
    public override string HtmlClose => "</h1>";

    public override Token? TryFindToken(string text, int idx)
    {
        var indexOfOpen = OpenIdx(text, idx);

        if (indexOfOpen == -1)
            return null;

        var indexOfClose = text.IndexOf(MdClose, idx, StringComparison.Ordinal);

        return indexOfClose == -1
            ? new HeaderToken(text.Substring(indexOfOpen, text.Length - 1 - indexOfOpen + MdClose.Length))
            : new HeaderToken(text.Substring(indexOfOpen, indexOfClose - indexOfOpen + MdClose.Length));
    }

    protected override bool IsOpenTag(string text, int idx)
    {
        return text.IsOpenOfParagraph(idx);
    }

    protected override bool IsCloseTag(string text, int idx)
    {
        return true;
    }
}