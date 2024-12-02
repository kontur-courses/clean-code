namespace Markdown.Extensions;

public static class QueueExtensions
{
    public static IEnumerable<T> Dequeue<T>(this Queue<T> queue, int count)
    {
        for (var i = 0; i < count; i++)
        {
            if (queue.Count == 0)
                break;
            
            yield return queue.Dequeue();
        }
    }
}