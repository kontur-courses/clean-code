using Markdown.Extensions;

namespace Markdown.Tokens;

public class LiteralToken:Token
{
    public override string TagWrapper { get; } = "";
    public override string Separator { get; } = "";
    public override bool IsCanContainAnotherTags { get; } = false;
    public override bool IsSingleSeparator { get; } = false;
    private string Content { get; set; }

    public LiteralToken(int openingIndex,int closingIndex,string content) :base(0,1)
    {
        if (openingIndex < 0 || openingIndex > closingIndex || string.IsNullOrEmpty(content))
            throw new ArgumentException();
        this.OpeningIndex = openingIndex;
        this.ClosingIndex = closingIndex;
        Content = content;
    }
    
    public override void Validate(string str)
    {
        IsCorrect = !(this.IsSeparatorsInsideDifferentWords(str) ||this.IsTokenHasNoContent());
    }
    
    public override string ToString()
    {
        return Content;
    }

}