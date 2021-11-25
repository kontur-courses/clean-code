using System.Text;

namespace Markdown.Render
{
    public interface IRenderer
    {
        public bool CanRender(TextType type);
        public string Render(HyperTextElement element);
    }
}