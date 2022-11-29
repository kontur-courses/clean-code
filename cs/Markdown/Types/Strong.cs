namespace Markdown.Types;

public class Strong : IType
{
    private string htmlTag = "strong";
    public string ConvertToHtml(string text)
    {
        string required = $"<{htmlTag}>" + text + $"</{htmlTag}>";
        return required;
    }
}