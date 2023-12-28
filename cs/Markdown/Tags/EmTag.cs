using Markdown.Tokens;

namespace Markdown.Tags;

public class EmTag : Tag
{
    public override string MdOpen => "_";
    public override string MdClose => "_";
    public override string HtmlOpen => "<em>";
    public override string HtmlClose => "</em>";

    public override Token? TryFindToken(string text, int idx)
    {
        var indexOfOpen = OpenIdx(text, idx);

        var indexOfClose = CloseIdx(text, idx);

        return indexOfOpen == -1 || indexOfClose == -1 || IsIntersectWithStrongTag(text, indexOfOpen, indexOfClose)
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

    protected override bool IsOpenTag(string text, int idx)
    {
        if (idx > 0 && text[idx - 1] == '_'
            || text.Length > idx + MdOpen.Length && text[idx + MdOpen.Length] == '_')
            return false;

        return (text.Length <= idx + MdOpen.Length || text[idx + MdOpen.Length] != ' ')
               && (idx == 0 || (!char.IsLetter(text[idx - 1]) && !char.IsDigit(text[idx - 1])));
    }

    protected override bool IsCloseTag(string text, int idx)
    {
        if (idx > 0 && text[idx - 1] == '_'
            || text.Length > idx + MdClose.Length && text[idx + MdClose.Length] == '_')
            return false;

        return text[idx - 1] != ' ' && (text.Length <= idx + MdClose.Length
                                        || !char.IsLetter(text[idx + MdClose.Length]) &&
                                        !char.IsDigit(text[idx + MdClose.Length]));
    }
}