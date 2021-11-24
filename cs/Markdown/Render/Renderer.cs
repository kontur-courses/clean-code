using System.Text;

namespace Markdown.Render
{
    public class Renderer : IRenderer
    {
        private readonly TextType renderType;
        private readonly string renderPrefix;
        private readonly string renderSuffix;

        public Renderer(TextType renderType, string renderPrefix, string renderSuffix)
        {
            this.renderType = renderType;
            this.renderPrefix = renderPrefix;
            this.renderSuffix = renderSuffix;
        }

        public bool CanRender(TextType type)
        {
            return renderType == type;
        }

        public void Render(HyperTextElement element, IRenderer[] renderers, StringBuilder currentRender)
        {
            currentRender.Append(renderPrefix);
            foreach (var child in element.Children)
            {
                foreach (var renderer in renderers)
                {
                    if (renderer.CanRender(child.Type))
                        renderer.Render(child, renderers, currentRender);
                }
            }
            if (element is HyperTextElement<string> valueElement)
                currentRender.Append(valueElement.Value);
            currentRender.Append(renderSuffix);
        }   
    }
}