namespace Markdown;

public class Tag
{
    public string Name { get; private set; }
    public string Content { get; private set; }

    public Tag(string name, string content)
    {
        Name = name;
        Content = content;
    }

    public override string ToString()
    {
        return $"<{Name}>{Content}</{Name}>";
    }
}