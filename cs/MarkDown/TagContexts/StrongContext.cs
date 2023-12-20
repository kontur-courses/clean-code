using MarkDown.TagContexts.Abstracts;
using MarkDown.Tags.Abstracts;

namespace MarkDown.TagContexts;

public class StrongContext : TagContext
{
    private bool IsInWord { get; }
    
    public StrongContext(int startIndex, bool isInWord, TagContext? parent, Tag tag) : base(startIndex, parent, tag)
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