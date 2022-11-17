using MarkdownRenderer.Abstractions;

namespace MarkdownRenderer.Implementations.MarkdownParsers;

public abstract class MarkdownInlineParser<TElem> : MarkdownElementParser<TElem>
    where TElem : IElement
{
    public override ElementParseType ParseType => ElementParseType.Inline;
    public abstract override string Prefix { get; }
    public abstract override string Postfix { get; }

    protected abstract Func<string, TElem> ElementCreator { get; }

    public override bool IsElementStart(string content, int index)
    {
        if (index + Prefix.Length + Postfix.Length >= content.Length)
            return false;
        if (Prefix.Where((c, i) => content[index + i] != c).Any())
            return false;
        if (content[index + Prefix.Length] is ' ' or '_')
            return false;

        return index == 0 || content[index - 1] is not '_';
    }

    public override bool IsElementEnd(string content, int index)
    {
        if (index - (Prefix.Length + Postfix.Length) < 0)
            return false;
        if (Postfix.Where((_, i) => content[index - Postfix.Length + i + 1] != Postfix[i]).Any())
            return false;
        if (content[index - Postfix.Length] is ' ' or '_')
            return false;

        return index + 1 >= content.Length || content[index + 1] is not '_';
    }

    public override bool TryParseElement(string content, Token contentToken, out TElem? element)
    {
        element = default;
        if (!IsElementStart(content, contentToken.Start) || !IsElementEnd(content, contentToken.End))
            return false;
        if (contentToken.Length < Prefix.Length + Postfix.Length + 1)
            return false;

        var rawContent = content.Substring(
            contentToken.Start + Prefix.Length,
            contentToken.Length - (Prefix.Length + Postfix.Length));
        if (
            IsSelectionInsideWord(content, contentToken) &&
            rawContent.Any(symbol => char.IsDigit(symbol) || symbol == ' ')
        )
            return false;

        element = ElementCreator(content);
        return true;
    }

    private static bool IsSelectionInsideWord(string content, Token contentToken)
    {
        if (contentToken.Start > 0 && content[contentToken.Start - 1] != ' ')
            return true;

        if (contentToken.End + 1 < content.Length && content[contentToken.End + 1] != ' ')
            return true;

        return false;
    }
}