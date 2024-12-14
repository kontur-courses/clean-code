namespace Markdown.Traversable;

public interface ITraversable<T>
{
    public IEnumerable<T> Traverse();
}