namespace Markdown;

public class Tag
{
    public string TagName { get; private set; }
    public string OpeningTag => $"<{TagName}>";
    public string ClosingTag => $"</{TagName}>";
    
    public Tag(string tagName) => this.TagName = tagName;
}