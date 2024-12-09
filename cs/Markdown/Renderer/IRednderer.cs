namespace Markdown.Rendered;

public interface IRenderer<T>
{
    public string Render(T input);
}