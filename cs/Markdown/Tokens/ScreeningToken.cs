namespace Markdown.Tokens;

public class ScreeningToken : Token
{
    public override string TagWrapper { get; } = "";
    public override string Separator { get; } = "\\";

    public override bool IsCanContainAnotherTags { get; } = false;
    public override bool IsSingleSeparator { get; } = true;
    public override bool IsContented { get; } = false;


    public override void Validate(string str, IEnumerable<Token> tokens)
    {
        IsCorrect = true;
    }
    
    public ScreeningToken(int openingIndex, int closingIndex) : base(openingIndex, closingIndex)
    {
    }

    public override string ToString()
    {
        return "\\";
    }
}