namespace MarkdownRenderer.Infrastructure;

public class LambdaComparer<T> : IComparer<T>
{
    private readonly Func<T, T, int> _compareFunction;

    public LambdaComparer(Func<T, T, int> compareFunction)
    {
        _compareFunction = compareFunction;
    }

    public int Compare(T? x, T? y)
    {
        if (x is null)
            throw new ArgumentNullException(nameof(x));
        if (y is null)
            throw new ArgumentNullException(nameof(y));

        return _compareFunction(x, y);
    }
}