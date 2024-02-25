namespace Markdown.Tokens;

public class ListItemToken : Token
{
    public override string TagWrapper { get; } = "li";
    public override string Separator { get; } = "*";
    public override bool IsCanContainAnotherTags { get; } = true;
    public override bool IsSingleSeparator { get; } = false;

    public override bool IsContented { get; } = true;


    public ListItemToken(int openingIndex, int closingIndex) : base(openingIndex, closingIndex)
    {
    }

    public ListItemToken(int openingIndex) : base(openingIndex)
    {
    }

    public override void Validate(string str, IEnumerable<Token> tokens)
    {
    }
}