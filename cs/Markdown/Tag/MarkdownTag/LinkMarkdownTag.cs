namespace Markdown;

public class LinkMarkdownTag() : MarkdownTag(TagType.Link, "[", true)
{
    public static readonly string StartTagTextForName = "[";
    public static readonly string EndTagTextForName = "]";
    public static readonly string StartTagTextForLink = "(";
    public static readonly string EndTagTextForLink = ")";
    public static readonly string StartTagTextForTooltip = " \"";
    public static readonly string EndTagTextForTooltip = "\"";
    
    public override bool IsStartOfTag(string text, int position, bool isSameTagAlreadyOpen)
    {
        if (TagText.Length + position > text.Length) return false;

        var isContainsStartTagTextForName = text.AsSpan(position, StartTagTextForName.Length).Equals(StartTagTextForName, StringComparison.Ordinal);

        return isContainsStartTagTextForName;
    }
    
    public static bool TryProcessTag(string text, int startPosition, out (TokenTagLink? token, int lengthTag) result)
    {
        var currentPosition = startPosition;
        
        if (!text.StartsWith(StartTagTextForName, StringComparison.Ordinal))
        {
            result = (null, 0);
            return false;
        }
        
        var indexEndTagTextForName = text.IndexOf(EndTagTextForName, startPosition + StartTagTextForName.Length, StringComparison.Ordinal);
        var indexStartTagTextForLink = text.IndexOf(StartTagTextForLink, indexEndTagTextForName + EndTagTextForName.Length, StringComparison.Ordinal);

        var indexStartTagTextForTooltip = -1;
        var indexEndTagTextForTooltip = -1;
        
        if (TryFindTooltip(text, currentPosition, out var tooltip))
        {
            indexStartTagTextForTooltip = tooltip.start;
            indexEndTagTextForTooltip = tooltip.end;
        }
        
        var indexEndTagTextForLink = text.IndexOf(EndTagTextForLink, indexStartTagTextForLink + StartTagTextForLink.Length, StringComparison.Ordinal);

        if (indexEndTagTextForName == -1 || indexStartTagTextForLink == -1 || indexEndTagTextForLink == -1)
        {
            result = (null, 0);
            return false;
        }

        string linkText;
        string? tooltipText = null;

        if (indexStartTagTextForTooltip == -1)
        {
            linkText = text.Substring(indexStartTagTextForLink + StartTagTextForLink.Length,
                indexEndTagTextForLink - indexStartTagTextForLink - StartTagTextForLink.Length); 
        }
        else
        {
            linkText = text.Substring(indexStartTagTextForLink + StartTagTextForLink.Length,
                indexStartTagTextForTooltip - indexStartTagTextForLink - StartTagTextForLink.Length);
            tooltipText = text.Substring(indexStartTagTextForTooltip + StartTagTextForTooltip.Length,
                indexEndTagTextForTooltip - indexStartTagTextForTooltip - StartTagTextForTooltip.Length);
        }


        var content = text.Substring(startPosition + StartTagTextForName.Length, indexEndTagTextForName - startPosition - EndTagTextForName.Length);
        
        result = (MarkdownParser.CreateLinkToken(content, linkText, tooltipText), indexEndTagTextForLink - startPosition + 1);
        return true;
    }

    private static bool TryFindTooltip(string text, int startPosition, out (int start, int end) positions)
    {
        var indexStartTagTextForTooltip = text.IndexOf(StartTagTextForTooltip, startPosition, StringComparison.Ordinal);
        var indexEndTagTextForTooltip = text.IndexOf(EndTagTextForTooltip, indexStartTagTextForTooltip + StartTagTextForTooltip.Length, StringComparison.Ordinal);
        
        if (indexEndTagTextForTooltip == -1 || indexStartTagTextForTooltip == -1)
        {
            positions = (0, 0);
            return false;
        }

        positions = (indexStartTagTextForTooltip, indexEndTagTextForTooltip);
        return true;
    }
}