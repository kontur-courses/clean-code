namespace MarkDown;

public class Pair
{
    public Subsequence Open;
    public Subsequence Close;
    public int Length => Open.Length;
    public bool IsAlive = true;
}