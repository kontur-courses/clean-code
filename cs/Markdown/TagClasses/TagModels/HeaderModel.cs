namespace Markdown.TagClasses.TagModels;

public class HeaderModel : TagModel
{
    public HeaderModel() : base("Header", "# ", null, true, false, "<h1>", "</h1>")
    {
    }
}