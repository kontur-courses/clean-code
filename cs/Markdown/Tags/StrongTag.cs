using Markdown.Tokens;

namespace Markdown.Tags;

public class StrongTag : Tag
{
    public override string MdOpen => "__";
    public override string MdClose => "__";
    public override string HtmlOpen => "<strong>";
    public override string HtmlClose => "</strong>";

    public override Token? TryFindToken(string text, int idx)
    {
        var indexOfOpen = OpenIdx(text, idx);

        var indexOfClose = CloseIdx(text, idx);

        return indexOfOpen == -1 || indexOfClose == -1 || IsIntersectWithEmTag(text, indexOfOpen, indexOfClose)
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

    protected override bool IsOpenTag(string text, int idx)
    {
        return (text.Length <= idx + MdClose.Length || text[idx + MdOpen.Length] != ' ')
               && (idx == 0 || !char.IsLetter(text[idx - 1]) && !char.IsDigit(text[idx - 1]));
    }

    protected override bool IsCloseTag(string text, int idx)
    {
        return text[idx - 1] != ' ' && (text.Length <= idx + MdClose.Length ||
                                        !char.IsLetter(text[idx + MdClose.Length]) &&
                                        !char.IsDigit(text[idx + MdClose.Length]));
    }
}