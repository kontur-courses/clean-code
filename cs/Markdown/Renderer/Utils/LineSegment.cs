namespace Markdown.Renderer.Utils;

public class LineSegment
{
    public int StartIndex { get; }

    public int EndIndex { get; }

    public LineSegment(int startIndex, int endIndex)
    {
        StartIndex = startIndex;
        EndIndex = endIndex;
    }
}