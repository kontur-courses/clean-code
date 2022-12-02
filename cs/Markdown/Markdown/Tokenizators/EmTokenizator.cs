namespace Markdown;

public class EmTokenizator : SimpleTagTokenizator
{
    public EmTokenizator() : base()
    {
    }

    public EmTokenizator(HashSet<int> usedIndexes) : base(usedIndexes)
    {
    }

    public override Tag Tag => new EmTag();
}