using Markdown.Tokens;

namespace Markdown.Tags;

public abstract class Tag
{
    public abstract string MdOpen { get; }
    public abstract string MdClose { get; }
    public abstract string HtmlOpen { get; }
    public abstract string HtmlClose { get; }

    public abstract Token? TryFindToken(string text, int idx);

    protected abstract bool IsOpenTag(string text, int idx);

    protected abstract bool IsCloseTag(string text, int idx);

    protected int OpenIdx(string text, int idx)
    {
        var indexOfOpen = text.IndexOf(MdOpen, idx, StringComparison.Ordinal);

        if (indexOfOpen != idx || !IsOpenTag(text, idx) || text.IsShielded(idx))
            return -1;

        return indexOfOpen;
    }

    protected int CloseIdx(string text, int idx)
    {
        var indexOfClose = 0;
        while (idx < text.Length)
        {
            indexOfClose = text.IndexOf(MdClose, idx + 1, StringComparison.Ordinal);

            if (indexOfClose == -1)
                return -1;

            if (text.IsShielded(idx) || !IsCloseTag(text, indexOfClose))
            {
                idx = indexOfClose;
                continue;
            }

            break;
        }

        return indexOfClose;
    }
}