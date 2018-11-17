using Markdown.Md;

namespace Markdown
{
    public interface IRenderer
    {
        string Render(MdToken[] tokens);
    }
}