namespace Markdown.Tags;

public class LinkTag : BaseTagHandler
{
    protected override string Symbol => "[";
    protected override string HtmlTag => "a";
    private string HtmlTagLink => " href=";
    public override bool IsTagStart(string text, int index) =>
        text[index].ToString() == Symbol;

    public override int ProcessTag(ref string text, int startIndex)
    {
        var endIndexOfNameLink = FindEndIndex(text, startIndex);
        if (endIndexOfNameLink == -1)
            return startIndex + 1;
        var nameLink = ExtractContent(text, startIndex, endIndexOfNameLink);
        nameLink = ProcessNestedTag(ref nameLink);
        var startIndexOfLink = FindStartIndexOfLink(text, endIndexOfNameLink);
        if (startIndexOfLink == -1)
            return startIndex + 1;
        var endIndexOfLink = FindEndIndexOfLink(text, endIndexOfNameLink);
        if (endIndexOfLink == -1)
            return startIndex + 1;
        var link = ExtractContent(text, startIndexOfLink, endIndexOfLink);
        var replacement = WrapWithHtmlTag(nameLink, link);
        if (HelperFunctions.ContainsOnlySpases(nameLink))
            replacement = "";
        text = ReplaceText(text, startIndex, endIndexOfLink, replacement);
        return startIndex + replacement.Length;

    }
    protected override int FindEndIndex(string text, int startIndex)
    {
        int bracketDepth = 0;
        for (int i = startIndex; i < text.Length; i++)
        {
            if (text[i] == '[')
                bracketDepth++;
            else if (text[i] == ']')
            {
                bracketDepth--;
                if (bracketDepth == 0)
                {
                    if (i + 1 < text.Length && text[i + 1] == '(')
                        return i;
                }
            }
        }
        return -1;
    }

    private int FindStartIndexOfLink(string text, int endIndexOfNameLink)
    {
        if (endIndexOfNameLink + 1 <= text.Length && text[endIndexOfNameLink + 1] == '(')
        {
            return endIndexOfNameLink + 1;
        }
        return -1;
    }

    private int FindEndIndexOfLink(string text, int endIndexOfNameLink)
    {
        var currentIndex = text.IndexOf(')', endIndexOfNameLink);
        return currentIndex;
    }

    protected string WrapWithHtmlTag(string contentName, string contentLink)
    {
        return $"<{HtmlTag}{HtmlTagLink}{contentLink}>{contentName}</{HtmlTag}>";
    }
}
