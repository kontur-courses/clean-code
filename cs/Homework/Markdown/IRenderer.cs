namespace Markdown
{
    public interface IRenderer
    {
        public string Render(Token[] tokens);
    }
}