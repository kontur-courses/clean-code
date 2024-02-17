namespace Markdown.Tokens;

public class ScreeningToken:Token
{
    public override string TagWrapper { get; } = "";
    public override string Separator { get; } = "\\";

    public override bool IsCanContainAnotherTags { get; } = false;
    public override bool IsSingleSeparator { get; } = true;
   
    public override void Validate(string str)
    {
        IsCorrect = true;
    }

    public ScreeningToken(int openingIndex, int closingIndex) : base(openingIndex, closingIndex) { }

    public override string ToString()
    {
        return "\\";
    }
}