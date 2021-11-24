using System.Text;

namespace Markdown.Render
{
    public interface IRenderer
    {
        public bool CanRender(TextType type);
        public void Render(HyperTextElement element, IRenderer[] renderers, StringBuilder currentRender);
    }
}