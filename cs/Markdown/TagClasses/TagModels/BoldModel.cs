namespace Markdown.TagClasses.TagModels;

public class BoldModel : TagModel
{
    public BoldModel() : base("Bold", "__", "__", true, false, "<strong>", "</strong>")
    {
    }
}