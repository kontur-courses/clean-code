namespace Markdown;

public class EmTokenizator : SimpleTagTokenizator
{
    public EmTokenizator() : base()
    {
    }

    public EmTokenizator(HashSet<int> usedIndexes) : base(usedIndexes)
    {
    }

    public override string OpenTag => MarkdownEmTag.OpenMdTag;
    public override string CloseTag => MarkdownEmTag.CloseMdTag;
}