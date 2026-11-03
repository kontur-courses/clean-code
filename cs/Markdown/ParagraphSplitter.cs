namespace Markdown;

internal readonly record struct Segment(string Gap, string Text);

internal static class ParagraphSplitter
{
    public static List<Segment> Split(string text)
    {
        throw new NotImplementedException();
    }
    
}