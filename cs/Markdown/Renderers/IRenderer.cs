namespace Markdown.Renderers
{
    public interface IRenderer
    {
        string Render(Tag tag);
    }
}