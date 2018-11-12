namespace Markdown
{
    public interface IRenderer
    {
        string Render(Token[] tokens);
    }
}