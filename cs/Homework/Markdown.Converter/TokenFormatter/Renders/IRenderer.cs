namespace Markdown.TokenFormatter.Renders
{
    public interface IRenderer
    {
        string RenderText(string text);
        string RenderImage(string src, string alt = "");
    }
}