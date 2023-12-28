using Markdown.Tokens;

namespace Markdown.Tags;

public class EmTag : ITag
{
    public string MdOpen => "_";
    public string MdClose => "_";
    public string HtmlOpen => "<em>";
    public string HtmlClose => "</em>";

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

        return IsIntersectWithStrongTag(text, indexOfOpen, indexOfClose)
            ? null
            : new EmToken(text.Substring(indexOfOpen, indexOfClose - indexOfOpen + MdClose.Length));
    }
    
    private bool IsIntersectWithStrongTag(string text, int indexOfOpen, int indexOfClose)
    {
        var tag = new StrongTag();
        for (var i = indexOfOpen + MdOpen.Length; i < indexOfClose; i++)
        {
            var strong = tag.TryFindToken(text, i);

            if (strong != null && i + strong.Str.Length >= indexOfClose)
            {
                TokenHighlighter.Excluded.Add(strong);
                return true;
            }
        }
        
        return false;
    }

    public bool IsOpenTag(string text, int idx)
    {
        if (idx > 0 && text[idx - 1] == '_' 
            || text.Length > idx + MdOpen.Length && text[idx + MdOpen.Length] == '_')
            return false;
        
        return (text.Length <= idx + MdOpen.Length || text[idx + MdOpen.Length] != ' ')
               && (idx == 0 || (!char.IsLetter(text[idx - 1]) && !char.IsDigit(text[idx - 1])));
    }

    public bool IsCloseTag(string text, int idx)
    {
        if (idx > 0 && text[idx - 1] == '_' 
            || text.Length > idx + MdClose.Length && text[idx + MdClose.Length] == '_')
            return false;
        
        return text[idx - 1] != ' ' && (text.Length <= idx + MdClose.Length 
            || !char.IsLetter(text[idx + MdClose.Length]) && !char.IsDigit(text[idx + MdClose.Length]));
    }
}