namespace Markdown.TagHandlers;

public abstract class BaseTagHandler : ITagHandler
{
    protected BaseTagHandler(string mdTag, string htmlTag)
    {
        if (!htmlTag.StartsWith("<") || !htmlTag.EndsWith(">"))
            throw new ArgumentException();
        MdTag = mdTag;
        HtmlTag = htmlTag;
    }

    public string MdTag { get; }
    public string HtmlTag { get; }

    protected abstract ITagHandler[] NestedTagHandlers { get; }

    public string Render(string text, int startIndex = 0)
    {
        if (!StartsWithTag(text, startIndex) || !IsValid(text, startIndex))
            return text;
        var endTagIndex = FindEndTagProcessing(text, startIndex);
        var processedText = RenderAndReplaceInnerContent(text, startIndex, endTagIndex);

        if (IntersectsWithAnyTags(text, startIndex, endTagIndex))
            return processedText + text[endTagIndex..];
        return Format(processedText) + text[endTagIndex..];
    }

    private string RenderAndReplaceInnerContent(string text, int startIndex, int endIndex)
    {
        var innerContent = GetInnerContent(text, startIndex);
        var renderedInnerContent = Md.Render(innerContent, NestedTagHandlers);
        return text[startIndex..endIndex].Replace(innerContent, renderedInnerContent);
    }

    private bool IntersectsWithAnyTags(string text, int startIndex, int endIndex)
    {
        for (var i = startIndex; i < endIndex;)
        {
            var handler = Md.FindHandler(text, i, NestedTagHandlers);
            if (handler != null)
            {
                var innerEndIndex = handler.FindEndTagProcessing(text, i);
                if (innerEndIndex > endIndex)
                    return true;
                i += handler.MdTag.Length;
            }
            else
            {
                i++;
            }
        }

        return false;
    }

    public void ValidateInput(string s, int startIndex)
    {
        if (!IsValid(s, startIndex))
            throw new ArgumentException();
    }

    public virtual bool StartsWithTag(string text, int startIndex)
    {
        return text[startIndex..].StartsWith(MdTag);
    }

    public abstract bool IsValid(string text, int startIndex = 0);

    public abstract int FindEndTagProcessing(string text, int startIndex);

    protected abstract string GetInnerContent(string s, int startIndex);

    protected abstract string Format(string s);
}