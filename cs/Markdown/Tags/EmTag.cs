namespace Markdown.Tags;

public class EmTag : ITag
{
    public string Md => "_";
    public string Html => "<em>";
}