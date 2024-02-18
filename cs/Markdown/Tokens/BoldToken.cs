using Markdown.Extensions;

namespace Markdown.Tokens;

public class BoldToken : Token
{
    public override string TagWrapper { get; } = "strong";
    public override string Separator { get; } = "__";
    public override bool IsCanContainAnotherTags { get; } = false;
    public override bool IsSingleSeparator { get; } = false;
    public override bool IsContented { get; } = false;

    public BoldToken(int openingIndex, int closingIndex) : base(openingIndex, closingIndex)
    {
    }

    public BoldToken(int openingIndex) : base(openingIndex)
    {
    }

    public override void Validate(string str, IEnumerable<Token> tokens)
    {
        IsCorrect = !(this.IsSeparatorsInsideDifferentWords(str) || this.IsTokenHasNoContent());
    }
}