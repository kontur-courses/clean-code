using Markdown.Tokens;

namespace Markdown.Tags;

public class HeaderTag : ITag
{
    public string MdOpen => "# ";
    public string MdClose => "\n";
    public string HtmlOpen => "<h1>";
    public string HtmlClose => "</h1>";

    public IToken? TryFindToken(string text, int idx)
    {
        var indexOfOpen = text.IndexOf(MdOpen, idx, StringComparison.Ordinal);

        if (indexOfOpen != idx || !IsOpenTag(text, idx) || text.IsShielded(idx))
            return null;

        var indexOfClose = text.IndexOf(MdClose, idx, StringComparison.Ordinal);

        return indexOfClose == -1
            ? new HeaderToken(text.Substring(indexOfOpen, text.Length - 1 - indexOfOpen + MdClose.Length))
            : new HeaderToken(text.Substring(indexOfOpen, indexOfClose - indexOfOpen + MdClose.Length));
    }

    public bool IsOpenTag(string text, int idx)
    {
        return text.IsOpenOfParagraph(idx);
    }

    public bool IsCloseTag(string text, int idx)
    {
        throw new NotImplementedException();
    }
}