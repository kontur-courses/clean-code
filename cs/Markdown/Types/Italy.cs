namespace Markdown.Types;

public class Italy : IType
{
    private string htmlTag = "em";
    
    public string ConvertToHtml(string text)
    {
        string required = $"<{htmlTag}>" + text + $"</{htmlTag}>";
        return required;
    }
}