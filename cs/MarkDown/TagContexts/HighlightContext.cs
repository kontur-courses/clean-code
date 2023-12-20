using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts;

public class HighlightContext : TagContext
{
    private bool IsInWord { get; }

    public HighlightContext(int startIndex, bool isInWord, TagContext? parent, TagFactory tagFactory) : base(startIndex, parent, tagFactory)
    {
        IsInWord = isInWord;
    }

    protected override void HandleSymbolItself(char symbol)
    {
        if (IsInWord && symbol == ' ' || char.IsDigit(symbol))
            ConsiderInCreatingHtml = false;
    }

    public override void CloseSingleTags(int closeIndex)
    {
        parent?.CloseSingleTags(closeIndex);
    }
}