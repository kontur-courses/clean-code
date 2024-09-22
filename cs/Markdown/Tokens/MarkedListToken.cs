using System.Collections;

namespace Markdown.Tokens;

public class MarkedListToken : Token
{
    public override string TagWrapper { get; } = "ul";
    public override string Separator { get; } = "|";
    public override bool IsCanContainAnotherTags { get; } = true;
    public override bool IsSingleSeparator { get; } = false;
    public override bool IsContented { get; } = false;


    public MarkedListToken(int openingIndex, int closingIndex) : base(openingIndex, closingIndex)
    {
    }

    public MarkedListToken(int openingIndex) : base(openingIndex)
    {
    }


    public override void Validate(string str, IEnumerable<Token> tokens)
    {
        var listItems = GetInsideRootTokens(tokens);
        if (!listItems.All(i => i is ListItemToken || (i is LiteralToken && string.IsNullOrWhiteSpace(i.ToString()))))
            return;
        foreach (var item in listItems)
        {
            item.IsCorrect = true;
        }

        IsCorrect = true;
    }

    private IEnumerable<Token> GetInsideRootTokens(IEnumerable<Token> tokens)
    {
        var insideTokens = tokens.Where(t => t.OpeningIndex > OpeningIndex && t.ClosingIndex < ClosingIndex);
        return insideTokens.Where(t1 =>
            !insideTokens.Any(
                t2 => t1 != t2 && t2.OpeningIndex <= t1.OpeningIndex && t2.ClosingIndex >= t1.ClosingIndex));
    }
}