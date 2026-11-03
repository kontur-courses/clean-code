namespace MarkDown;

public class Pair
{
    private Subsequence _open { get; }
    private Subsequence _close { get; }

    public Pair(Subsequence open, Subsequence close)
    {
        _open = open;
        _close = close;
    }
}