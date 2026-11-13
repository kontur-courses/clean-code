namespace Markdown.EmphasisResolving;

internal sealed class Scope
{
    public Stack<int> OpenedEm { get; } = new();
    public Stack<int> OpenedStrong { get; } = new();
    public List<(int Open, int Close)> StrongPairs { get; } = new();
}