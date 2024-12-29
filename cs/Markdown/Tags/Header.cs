namespace Markdown.Tags;

public class Header(string markdownText, int tagStart) : Tag(markdownText, tagStart)
{
    protected override string MdTag => "# ";
    protected override string HtmlTag => "h1";
    public override MdTagType TagType => MdTagType.Header;

    public override bool AcceptIfContextEnd(int currentPosition)
    {
        return currentPosition > MarkdownText.Length - 1 || MarkdownText[currentPosition] == '\n';
    }

    public override bool AcceptIfContextCorrect(int currentPosition)
    {
        return TagStart == 0 || TagStart - 1 == '\n';
    }

    public override void TryCloseTag(int contextEnd, string sourceMdText, out int tagEnd, List<Tag>? nested = null)
    {
        var contextStart = TagStart + MdTag.Length;
        tagEnd = contextEnd == sourceMdText.Length - 1 ? contextEnd : contextEnd + 1;
        Context = new Token(contextStart, sourceMdText, contextEnd - contextStart);
        NestedTags = nested;
        TagEnd = tagEnd;
    }
}