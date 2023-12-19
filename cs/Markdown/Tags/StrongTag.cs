namespace Markdown.Tags;

public class StrongTag : ITag
{
    public string Md => "__";
    public string Html => "<strong>";
}