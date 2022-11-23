namespace Markdown.Types;

public interface IType
{
    public string ConvertToHtml(string text);
}