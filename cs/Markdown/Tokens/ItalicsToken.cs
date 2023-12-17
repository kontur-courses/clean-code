namespace Markdown.Tokens;

public class ItalicsToken:Token
{
    protected override string TagWrapper { get; } = "/<em>";
    protected override string Separator { get; } = "_";
    protected override bool IsCanContainAnotherTags { get; } = false;
    protected override bool IsSingleSeparator { get; } = true;
    
    public ItalicsToken(int openingIndex, int closingIndex) : base(openingIndex, closingIndex) {}

    
}