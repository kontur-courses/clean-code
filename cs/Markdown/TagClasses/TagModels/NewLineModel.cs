namespace Markdown.TagClasses.TagModels;

public class NewLineModel : TagModel
{
    public NewLineModel(string newLineSymbol) : base("NewLine", newLineSymbol, null, true, true, null, null)
    {
    }
}