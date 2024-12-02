namespace Markdown.Helpers;

public static class IntervalHelper
{
    public static bool IsIntersects((int left, int right) interval1, (int left, int right) interval2)
    {
        if (interval1.left > interval2.left)
            (interval1, interval2) = (interval2, interval1);
        
        return interval1.right > interval2.left && 
               interval1.right < interval2.right &&
               interval2.left > interval1.left;
    }

    public static bool IsPartOfInterval((int left, int right) interval, (int left, int right) innerInterval) =>
        interval.left <= innerInterval.left && interval.right >= innerInterval.right;
    
    public static bool IsPointInInterval((int left, int right) interval, int position) => 
        position > interval.left && position < interval.right;
}