namespace Markdown.Tags;

public class HeaderTag : ISingleTag
{
    public string Md => "#";
    public string Html => "<h1>";
}