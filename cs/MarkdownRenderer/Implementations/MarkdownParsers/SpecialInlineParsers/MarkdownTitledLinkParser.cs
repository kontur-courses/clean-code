using MarkdownRenderer.Implementations.Elements;
using MarkdownRenderer.Infrastructure;

namespace MarkdownRenderer.Implementations.MarkdownParsers.SpecialInlineParsers;

public class MarkdownTitledLinkParser : MarkdownSpecialInlineElementParser<LinkElement>
{
    public override string Prefix => "[";
    public override string Postfix => ")";
    protected override Func<string, LinkElement> ElementCreator => null!;

    public override bool TryParseElement(string content, Token contentToken, out LinkElement? element)
    {
        element = default;

        if (!TryParseLinkParts(content.Substring(contentToken), out var title, out var destination))
            return false;

        element = new LinkElement(destination!, title!);
        return true;
    }

    private bool TryParseLinkParts(string linkContent, out string? title, out string? destination)
    {
        title = default;
        destination = default;

        if (linkContent.Length < 5)
            return false;

        if (!IsElementStart(linkContent, 0) || !IsElementEnd(linkContent, linkContent.Length - 1))
            return false;

        var titleClosingBraceIndex = linkContent.IndexOf(']');
        if (titleClosingBraceIndex == -1)
            return false;
        
        if (linkContent[titleClosingBraceIndex + 1] != '(')
            return false;

        title = linkContent[1..titleClosingBraceIndex];
        destination = linkContent[(titleClosingBraceIndex + 2)..^1];

        return LinkElement.IsUriCorrect(destination);
    }
}