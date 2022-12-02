namespace Markdown;

public class StrongTokenizator : SimpleTagTokenizator
{
    public StrongTokenizator() : base()
    {
    }

    public StrongTokenizator(HashSet<int> usedIndexes) : base(usedIndexes)
    {
    }

    public override string OpenTag => MarkdownStrongTag.OpenMdTag;
    public override string CloseTag => MarkdownStrongTag.CloseMdTag;
    public override Tag Tag => new StrongTag();
}