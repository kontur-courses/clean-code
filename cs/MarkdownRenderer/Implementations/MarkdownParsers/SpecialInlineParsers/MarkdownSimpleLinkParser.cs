using System.Text.RegularExpressions;
using System.Text.Unicode;
using MarkdownRenderer.Implementations.Elements;
using MarkdownRenderer.Infrastructure;

namespace MarkdownRenderer.Implementations.MarkdownParsers.SpecialInlineParsers;

public class MarkdownSimpleLinkParser : MarkdownSpecialInlineElementParser<LinkElement>
{
    public override string Prefix => "<";
    public override string Postfix => ">";
    protected override Func<string, LinkElement> ElementCreator => null!;

    public override bool TryParseElement(string content, Token contentToken, out LinkElement? element)
    {
        element = default;
        if (contentToken.Length < Prefix.Length + Postfix.Length + 1)
            return false;
        if (!IsElementStart(content, contentToken.Start) || !IsElementEnd(content, contentToken.End))
            return false;

        var destination = content.Substring(
            contentToken.Start + Prefix.Length,
            contentToken.Length - (Prefix.Length + Postfix.Length)
        );

        if (!LinkElement.IsUriCorrect(destination))
            return false;

        element = new LinkElement(destination);
        return true;
    }
}