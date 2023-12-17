namespace Markdown.Tags;

public class EmTag : IPairTag
{
    public string Md => "_";
    public string Html => "<em>";
}