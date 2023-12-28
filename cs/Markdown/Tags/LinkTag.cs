using Markdown.Tokens;

namespace Markdown.Tags;

public class LinkTag : Tag
{
    public override string MdOpen => "[";
    public override string MdClose => ")";
    public override string HtmlOpen => "<a href=\"";
    public override string HtmlClose => "</a>";

    public override Token? TryFindToken(string text, int idx)
    {
        var indexOfOpen = OpenIdx(text, idx);

        var indexOfClose = text.IndexOf(MdClose, idx, StringComparison.Ordinal);

        return indexOfOpen == -1 || indexOfClose == -1 || !IsLinkTag(text, indexOfOpen, indexOfClose)
            ? null
            : new LinkToken(text.Substring(indexOfOpen, indexOfClose - indexOfOpen + MdClose.Length));
    }

    private bool IsLinkTag(string text, int indexOfOpen, int indexOfClose)
    {
        return text.Substring(indexOfOpen, indexOfClose - indexOfOpen).Contains("](");
    }

    protected override bool IsOpenTag(string text, int idx)
    {
        return true;
    }

    protected override bool IsCloseTag(string text, int idx)
    {
        return true;
    }
}