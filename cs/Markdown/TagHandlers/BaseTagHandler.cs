namespace Markdown.TagHandlers;

public abstract class BaseTagHandler : ITagHandler
{
    protected BaseTagHandler(string tag)
    {
        Tag = tag;
    }

    public string Tag { get; }

    protected abstract ITagHandler[] NestedTagHandlers { get; }

    public string Render(string s, int startIndex = 0)
    {
        if (StartsWithTag(s, startIndex) && IsValid(s, startIndex) && !SymbolIsEscaped(s, startIndex))
        {
            var endTagIndex = FindEndTagProcessing(s, startIndex);
            var innerContent = GetInnerContent(s, startIndex);
            var renderedInnerContent = Md.Render(innerContent, NestedTagHandlers);
            var processedText = s[startIndex..endTagIndex].Replace(innerContent, renderedInnerContent);
            return Format(processedText) + s[endTagIndex..];
        }
        return s;
    }

    public abstract bool StartsWithTag(string s, int startIndex);

    public abstract bool IsValid(string s, int startIndex = 0);

    public abstract int FindEndTagProcessing(string s, int startIndex);

    protected abstract string GetInnerContent(string s, int startIndex);

    protected abstract string Format(string s);

    public static bool SymbolIsEscaped(string s, int index)
    {
        if (index < 0 || index >= s.Length)
            throw new ArgumentOutOfRangeException();
        return index != 0 && s[index - 1] == '\\';
    }
}