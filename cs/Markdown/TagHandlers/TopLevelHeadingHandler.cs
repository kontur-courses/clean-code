using Markdown.Extensions;

namespace Markdown.TagHandlers;

public class TopLevelHeadingHandler : ITagHandler
{
    public string ClosingHtmlTag => "</h1>";
    public KeyValuePair<string, string> MdHtmlTagPair => new("#", "<h1>");
    
    public bool CanTransform(string text)
    {
        var mdTag = MdHtmlTagPair.Key;
        return text.Length > mdTag.Length && text.StartsWith(mdTag) && text[mdTag.Length] == ' ';
    }

    public StringManipulator Transform(string text)
    {
        if (text is null)
            throw new ArgumentNullException(nameof(text));
        
        if (!CanTransform(text))
            return StringManipulator.Default(text);

        var end = FindEnd(text);
        var htmlTag = MdHtmlTagPair.Value;
        var transformed = htmlTag + GetInnerString(text) + ClosingHtmlTag;
        
        return new StringManipulator(transformed, text[..end], htmlTag.Length,
            transformed.Length - ClosingHtmlTag.Length);
    }

    private int FindEnd(string text)
    {
        if (!CanTransform(text))
            return -1;
        
        for (var i = 0; i < text.Length; i++)
        {
            if (text[i] is '\n' && !text.IsEscaped(i))
                return i;
        }

        return text.Length;
    }

    private string GetInnerString(string text)
    {
        if (!CanTransform(text))
            throw new ArgumentException(nameof(text));
        
        var start = MdHtmlTagPair.Key.Length + 1;
        var end = FindEnd(text);
        
        return text[start..end].Trim();
    }
}