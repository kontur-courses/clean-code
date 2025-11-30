namespace Markdown;

public class HeaderMarkdownTag() : MarkdownTag(TagType.Header, "# ", false)
{
    public override bool IsStartOfTag(string text, int position, bool isSameTagAlreadyOpen)
    {
        if (TagText.Length + position > text.Length) return false;

        var doubleNewLine = string.Concat(Enumerable.Repeat(Environment.NewLine, 2));
        var length = doubleNewLine.Length;

        var isNewParagraph = position == 0 || (position >= length && text.AsSpan(position - length, length)
            .Equals(doubleNewLine, StringComparison.Ordinal));

        return text.AsSpan(position, TagText.Length).Equals(TagText, StringComparison.Ordinal) && isNewParagraph &&
               !isSameTagAlreadyOpen;
    }

    public static void ProcessEndTag(string text, Stack<OpenToken> tokensWithOpenTag, int position,
        List<Token> result)
    {
        if (tokensWithOpenTag.All(t => t.OpenTag.TagType != TagType.Header))
            return;

        while (!tokensWithOpenTag.IsPeekEqual(TagType.Header))
            tokensWithOpenTag.Pop();

        if (tokensWithOpenTag.Count > 0)
        {
            var headerToken = tokensWithOpenTag.Pop();
            MarkdownParser.AddToken(MarkdownParser.CreateToken(text, position, headerToken), tokensWithOpenTag, result);
        }
    }
}