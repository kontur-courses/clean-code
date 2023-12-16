namespace Markdown.TagHandlers;

public abstract class PairedTagsHandler : BaseTagHandler
{
    protected PairedTagsHandler(string mdTag, string htmlTag) : base(mdTag, htmlTag)
    {
    }

    public override bool IsValid(string s, int startIndex = 0)
    {
        return !string.IsNullOrWhiteSpace(s)
               && startIndex >= 0
               && startIndex < s.Length
               && s.Length - startIndex - 1 >= MdTag.Length + 2
               && StartsWithTag(s, startIndex)
               && HasValidInnerContent(s, startIndex);
    }

    private bool HasValidInnerContent(string s, int startIndex)
    {
        var endIndex = s.Substring(startIndex + MdTag.Length).IndexOf(MdTag);
        if (endIndex == -1)
            return false;
        var inner = s.Substring(startIndex + MdTag.Length, endIndex);
        return inner.Trim().Length > 0 && inner.Trim().Length == inner.Length;
    }

    public override int FindEndTagProcessing(string s, int startIndex)
    {
        ValidateInput(s, startIndex);
        for (var i = startIndex + MdTag.Length; i < s.Length; i++)
        {
            var indexClosingTag = s[i..].IndexOf(MdTag);
            if (indexClosingTag != -1)
                return indexClosingTag + i + MdTag.Length;
        }

        return s.Length;
    }

    protected override string GetInnerContent(string s, int startIndex)
    {
        ValidateInput(s, startIndex);
        var end = FindEndTagProcessing(s, startIndex);
        return s.Substring(startIndex + MdTag.Length, end - (startIndex + MdTag.Length) - MdTag.Length);
    }

    protected override string Format(string s)
    {
        var closingTag = HtmlTag.Insert(1, "/");
        return $"{HtmlTag}{GetInnerContent(s, 0)}{closingTag}";
    }
}