namespace Markdown.Tokens;

public class LiteralToken:Token
{
    protected override string TagWrapper { get; } = "";
    protected override string Separator { get; } = "";
    protected override bool IsCanContainAnotherTags { get; } = false;
    protected override bool IsSingleSeparator { get; } = false;
    private string Content { get; set; }

    public LiteralToken(int openingIndex,int closingIndex,string content) :base(0,1)
    {
        if (openingIndex < 0 || openingIndex > closingIndex || string.IsNullOrEmpty(content))
            throw new ArgumentException();
        this.OpeningIndex = openingIndex;
        this.ClosingIndex = closingIndex;
        Content = content;
        
    }
    
    
    public override string GetTokenContent()
    {
        return Content;
    }

}