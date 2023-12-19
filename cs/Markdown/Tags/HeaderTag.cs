namespace Markdown.Tags;

public class HeaderTag : ITag
{
    public string Md => "# ";
    public string Html => "<h1>";
}