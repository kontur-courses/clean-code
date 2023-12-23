using Markdown.Extensions;

namespace Markdown.TagHandlers;

public class PairedTagHandler : ITagHandler
{
    public KeyValuePair<string, string> MdHtmlTagPair { get; }
    public string ClosingHtmlTag => MdHtmlTagPair.Value.Insert(1, "/");

    public PairedTagHandler(string mdTag, string htmlTag)
    {
        if (mdTag.Length < 1)
            throw new ArgumentException(nameof(mdTag));

        MdHtmlTagPair = new KeyValuePair<string, string>(mdTag, htmlTag);
    }


    public bool CanTransform(string text)
    {
        var mdTag = MdHtmlTagPair.Key;
        if (string.IsNullOrEmpty(text) || !text.StartsWith(mdTag))
            return false;

        var end = FindEnd(text);
        if (end == -1)
            return false;

        var innerString = text.Substring(mdTag.Length, end - mdTag.Length * 2);
        return innerString.Length != 0 && innerString.Trim().Length == innerString.Length;
    }

    public StringManipulator Transform(string text)
    {
        if (text is null)
            throw new ArgumentNullException();

        if (!CanTransform(text))
            return StringManipulator.Default(text);

        var htmlTag = MdHtmlTagPair.Value;
        var end = FindEnd(text);
        var transformed = htmlTag + GetInnerString(text) + ClosingHtmlTag;

        return new StringManipulator(transformed, text[..end], htmlTag.Length,
            transformed.Length - ClosingHtmlTag.Length);
    }

    private int FindEnd(string text)
    {
        var mdTag = MdHtmlTagPair.Key;
        for (var i = mdTag.Length; i < text.Length; i++)
        {
            if (!text.IsEscaped(i) && text[i..].StartsWith(mdTag))
                return i + mdTag.Length;
        }

        return -1;
    }

    private string GetInnerString(string text)
    {
        if (!CanTransform(text))
            throw new ArgumentException(nameof(text));

        var mdTag = MdHtmlTagPair.Key;
        var end = FindEnd(text) - mdTag.Length;

        return text[mdTag.Length..end];
    }
}