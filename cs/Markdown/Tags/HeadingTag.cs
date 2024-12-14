namespace Markdown.Tags;
public class HeadingTag : BaseTagHandler
{
    protected override string Symbol => "#";
    protected override string HtmlTag => "h1";

    public override bool IsTagStart(string text, int index)
    {
        return (text[index].ToString() == Symbol && index + 1 < text.Length &&
        char.IsWhiteSpace(text[index + 1])) || HelperFunctions.ContainsOnlyHeading(text);
    }

    public override int ProcessTag(ref string text, int startIndex)
    {
        var endIndex = FindEndIndex(text, startIndex);
        var content = ExtractContent(text, startIndex, endIndex);
        content = HelperFunctions.RemoveExtraSpaces(content);
        if (HelperFunctions.ContainsOnlySpases(content) || HelperFunctions.ContainsOnlyHeading(content))
            return StringOnlySpaces(ref text, endIndex, 0);

        content = ProcessNestedTag(ref content);
        var replacement = WrapWithHtmlTag(content);
        text = ReplaceText(text, startIndex, endIndex, replacement);

        return startIndex + replacement.Length;
    }

    protected override string ProcessNestedTag(ref string text) => 
        HelperFunctions.ProcessNestedTag(ref text);

    protected override string ExtractContent(string text, int startIndex, int endIndex)
    {
        startIndex = FindStartIndex(text, startIndex + 1, endIndex);
        if (startIndex == -1)
            return "";
        return text.Substring(startIndex, endIndex - startIndex);
    }

    protected override int FindEndIndex(string text, int startIndex)
    {
        var endIndex = text.IndexOf('\n', startIndex + 1);
        return endIndex == -1 ? text.Length : endIndex;
    }

    private int FindStartIndex(string text, int startIndex, int endIndex)
    {
        if (string.IsNullOrWhiteSpace(text.Substring(startIndex)))
            return -1;

        for (int i = startIndex; i < endIndex; i++)
        {
            if (!char.IsWhiteSpace(text[i]))
                return i;
        }
        return -1;
    }

    protected override string ReplaceText(string text, int startIndex, int endIndex, string replacement)
    {
        if (endIndex < text.Length && text[endIndex] == '\n')
            return text.Substring(0, startIndex) + replacement + '\n' + text.Substring(endIndex + 1);
        
        return text.Substring(0, startIndex) + replacement + text.Substring(endIndex);
    }
}
