namespace Markdown.Tokens;

public class BoldToken:Token
{
    protected override string TagWrapper { get; } = "/<strong>";
    protected override string Separator { get; } = "__";
    protected override bool IsCanContainAnotherTags { get; } = true;
    protected override bool IsSingleSeparator { get; } = true;
    public BoldToken(int openingIndex, int closingIndex) : base(openingIndex,closingIndex){}
    
}