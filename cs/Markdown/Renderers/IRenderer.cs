namespace Markdown.Renderers
{
    public interface IRenderer
    {
        string Render(ITokenNode tokenNode);
    }
}