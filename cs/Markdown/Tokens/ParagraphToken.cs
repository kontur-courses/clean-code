namespace Markdown.Tokens;

public class ParagraphToken:Token
{
    protected override string TagWrapper { get; } = "h1";
    protected override string Separator { get; } = "#";
    protected override bool IsCanContainAnotherTags { get; } = true;
    protected override bool IsSingleSeparator { get; } = true;

    public ParagraphToken(int openingIndex, int closingIndex) : base(openingIndex, closingIndex) {}
    public ParagraphToken(int openingIndex) : base(openingIndex){}

  
}