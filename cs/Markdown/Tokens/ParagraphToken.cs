namespace Markdown.Tokens;

public class ParagraphToken:Token
{
    public override string TagWrapper { get; } = "h1";
    public override string Separator { get; } = "# ";
    public override bool IsCanContainAnotherTags { get; } = true;
    public override bool IsSingleSeparator { get; } = true;

    public ParagraphToken(int openingIndex, int closingIndex) : base(openingIndex, closingIndex) {}
    public ParagraphToken(int openingIndex) : base(openingIndex){}

    public override void Validate(string str)
    {
        IsCorrect =  OpeningIndex == 0 || str[OpeningIndex - 1] == '\n';
    }
  
}