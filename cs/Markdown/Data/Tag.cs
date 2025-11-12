namespace Markdown.Data;

public class Tag
{
    public string Name { get; }
    public string OpenTag => $"<{Name}>";
    public string CloseTag => $"</{Name}>";

    public Tag(string name)
    {
        Name = name;
    }
}