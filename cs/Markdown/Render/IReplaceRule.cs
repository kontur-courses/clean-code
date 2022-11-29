namespace Markdown.Render;

public interface IReplaceRule
{
    string TagId { get; }
    public string ApplyRule(string text);
}