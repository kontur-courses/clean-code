namespace Markdown.Types;

public class NoneType : IType
{
    private string htmlTag = "";
    
    public string ConvertToHtml(string text)
    {
        return text;
    }
}