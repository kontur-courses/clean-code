namespace Markdown;

public class StrongTokenizator : SimpleTagTokenizator
{
    public StrongTokenizator() : base()
    {
    }

    public StrongTokenizator(HashSet<int> usedIndexes) : base(usedIndexes)
    {
    }

    public override Tag Tag => new StrongTag();
}