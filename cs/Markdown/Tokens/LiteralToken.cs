namespace Markdown.Tokens;

public class LiteralToken:Token
{
    protected override string TagWrapper { get; } = "";
    protected override string Separator { get; } = "";
    protected override bool IsCanContainAnotherTags { get; } = false;
    protected override bool IsSingleSeparator { get; } = true;
    public string Content { get; private set; }

    public LiteralToken(int openingIndex,int closingIndex,string content):base(openingIndex,closingIndex)
    {
        Content = content;
    }
    

}