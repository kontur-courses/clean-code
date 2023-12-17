namespace Markdown.Tags;

public class HeaderTag : IUnpairTag
{
    public string Md => "#";
    public string Html => "<h1>";
}