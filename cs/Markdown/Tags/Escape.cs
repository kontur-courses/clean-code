namespace Markdown.Tags;

public class Escape(string markdownText, int tagStart) : Tag(markdownText, tagStart)
{
    protected override string MdTag => "\\";
    protected override string HtmlTag => "";
    private Dictionary<string, string> specSymbolsRendering = new Dictionary<string, string> { 
        { "n", "\n" }, { "t", "\t" } };
    public override MdTagType TagType => MdTagType.Escape;

    public override string RenderToHtml()
    {
        var value = Context.GetValue();
        return $"{(specSymbolsRendering.ContainsKey(value) ? specSymbolsRendering[value] : value)}";
    }

    public override bool AcceptIfContextEnd(int currentPosition)
    {
        return currentPosition > TagStart + 1;
    }

    public override bool AcceptIfContextCorrect(int currentPosition)
    {
        var current = MarkdownText[currentPosition];
        return base.AcceptIfContextCorrect(currentPosition)
               && currentPosition < MarkdownText.Length 
               && (Md.MdTags.ContainsKey(current) || specSymbolsRendering.ContainsKey(current.ToString()));
    }

    public override void TryCloseTag(int contextEnd, string sourceMdText, out int tagEnd, List<Tag>? nested = null)
    {
        Context = Md.MdTags.ContainsKey(MarkdownText[TagStart + 1]) || specSymbolsRendering.ContainsKey(MarkdownText[TagStart + 1].ToString())
            ? new Token(TagStart + 1, MarkdownText, 1)
            : new Token(TagStart, MarkdownText, 1);
        tagEnd = contextEnd;
        TagEnd = tagEnd;
    }
}