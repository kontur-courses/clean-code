namespace Markdown;

public class Tag : ICloneable
{
    public string GlobalMark { get; set; }
    public string OpenMark { get; set; }
    public string CloseMark { get; set; }
    public bool ShouldClose { get; set; }

    public Tag(string globalMark, string openMark, string closeMark, bool shouldClose)
    {
        GlobalMark = globalMark;
        OpenMark = openMark;
        CloseMark = closeMark;
        ShouldClose = shouldClose;
    }

    public object Clone()
    {
        return new Tag(GlobalMark, OpenMark, CloseMark, ShouldClose);
    }
}