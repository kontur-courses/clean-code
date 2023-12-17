namespace Markdown.Tags;

public class StrongTag : IPairTag
{
    public string Md => "__";
    public string Html => "<strong>";
}