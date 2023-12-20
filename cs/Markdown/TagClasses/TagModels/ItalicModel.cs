namespace Markdown.TagClasses.TagModels;

public class ItalicModel : TagModel
{
    public ItalicModel() : base("Italic", "_", "_", true, false, "<em>", "</em>")
    {
    }
}