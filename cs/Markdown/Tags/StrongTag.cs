using Markdown.Tokens;

namespace Markdown.Tags;

public class StrongTag : ITag
{
    public string MdOpen => "__";
    public string MdClose => "__";
    public string HtmlOpen => "<strong>";
    public string HtmlClose => "</strong>";

    public IToken? TryFindToken(string text, int idx)
    {
        var indexOfOpen = text.IndexOf(MdOpen, idx, StringComparison.Ordinal);

        if (indexOfOpen != idx || !IsOpenTag(text, idx) || text.IsShielded(idx))
            return null;

        var indexOfClose = 0;
        while (idx < text.Length)
        {
            indexOfClose = text.IndexOf(MdClose, idx + 1, StringComparison.Ordinal);

            if (indexOfClose == -1)
                return null;

            if (text.IsShielded(idx) || !IsCloseTag(text, indexOfClose))
            {
                idx = indexOfClose;
                continue;
            }

            break;
        }

        return IsIntersectWithEmTag(text, indexOfOpen, indexOfClose)
            ? null
            : new StrongToken(text.Substring(indexOfOpen, indexOfClose - indexOfOpen + MdClose.Length));
    }

    private bool IsIntersectWithEmTag(string text, int indexOfOpen, int indexOfClose)
    {
        var tag = new EmTag();
        for (var i = indexOfOpen + MdOpen.Length; i < indexOfClose; i++)
        {
            var em = tag.TryFindToken(text, i);

            if (em != null && i + em.Str.Length >= indexOfClose)
            {
                TokenHighlighter.Excluded.Add(em);
                return true;
            }
        }
        
        return false;
    }

    public bool IsOpenTag(string text, int idx)
    {
        return (text.Length <= idx + MdClose.Length || text[idx + MdOpen.Length] != ' ')
               && (idx == 0 || !char.IsLetter(text[idx - 1]) && !char.IsDigit(text[idx - 1]));
    }

    public bool IsCloseTag(string text, int idx)
    {
        return text[idx - 1] != ' ' && (text.Length <= idx + MdClose.Length ||
                                        !char.IsLetter(text[idx + MdClose.Length]) &&
                                        !char.IsDigit(text[idx + MdClose.Length]));
    }
}