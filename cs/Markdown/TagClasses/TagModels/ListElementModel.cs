namespace Markdown.TagClasses.TagModels;

public class ListElementModel : TagModel
{
    public ListElementModel() : base("ListElement", "\t", null, true, false, "<li>", "</li>")
    {
    }
}