namespace Markdown.Tokens;

public abstract class DoubleToken : Token
{
    public DoubleToken(string opening, string ending, TokenType type, Token? parent = null)
        : base(opening, ending, type)
    {
        Opening = opening;
        Ending = ending;
    }

    public override bool CanStartsHere(string text, int index)
    {
        return (base.CanStartsHere(text, index)
                && !text.IsInBound(index + Opening.Length)) || (text[index + Opening.Length] != ' '
                                                                && !text.IsInNumericWord(index));
    }

    public override bool CanEndsHere(string text, int index)
    {
        if (index == FirstPosition + Opening.Length || !base.CanEndsHere(text, index))
            return false;

        if (IsInWord(text, index))
            return !text.IsInNumericWord(index)
                   && !text.HasInRange(FirstPosition, index + Ending.Length - 1, ' ');

        if (!text.IsInBound(index - 1) || text[index - 1] == ' ')
            return false;

        return true;
    }

    protected bool IsInWord(string text, int endingStart)
    {
        var isOpeningInWord = text.IsRangeNotBoundedWith(FirstPosition, Opening.Length, ' ');
        var isEndingInWord = text.IsRangeNotBoundedWith(endingStart + Ending.Length, Ending.Length, ' ');
        var isOpeningAtStartWord = text.IsRangeAtStartOfWord(FirstPosition, Opening.Length);
        var isEndingAtEndWord = text.IsRangeAtEndOfWord(endingStart + Ending.Length, Ending.Length);
        return !(isOpeningAtStartWord && isEndingAtEndWord) && (isOpeningInWord || isEndingInWord);
    }
}