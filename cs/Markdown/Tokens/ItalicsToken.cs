using Markdown.Extensions;

namespace Markdown.Tokens;

public class ItalicsToken : Token
{
    public override string TagWrapper { get; } = "em";
    public override string Separator { get; } = "_";
    public override bool IsCanContainAnotherTags { get; } = false;
    public override bool IsSingleSeparator { get; } = false;
    public override bool IsContented { get; } = false;


    public ItalicsToken(int openingIndex, int closingIndex) : base(openingIndex, closingIndex)
    {
    }

    public ItalicsToken(int openingIndex) : base(openingIndex)
    {
    }

    public override void Validate(string str, IEnumerable<Token> tokens)
    {
        IsCorrect = !this.IsSeparatorsInsideDifferentWords(str);
    }
}