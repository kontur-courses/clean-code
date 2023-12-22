namespace Markdown;

public class NeighboursContext
{
    public NeighboursContext(char? left, char? right)
    {
        Left = left;
        Right = right;
    }

    public char? Left { get; }
    public char? Right { get; }
}