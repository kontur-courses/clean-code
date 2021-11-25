using System;
using System.Collections.Generic;
using System.Text;

namespace Markdown.Render
{
    public class HtmlRender : IRenderer
    {
        private readonly Dictionary<TextType, RenderInfo> renderInfo;
        public static readonly HtmlRender Default = new HtmlRender();
        private HtmlRender()
        {
            renderInfo = new Dictionary<TextType, RenderInfo>()
            {
                [TextType.Body] = new RenderInfo(TextType.Body, "", ""),
                [TextType.Header] = new RenderInfo(TextType.Header, "<h1>", "</h1>\n"),
                [TextType.Paragraph] = new RenderInfo(TextType.Paragraph, "<p>", "</p>\n"),
                [TextType.BoldText] = new RenderInfo(TextType.BoldText, "<strong>", "</strong>"),
                [TextType.ItalicText] = new RenderInfo(TextType.ItalicText, "<em>", "</em>"),
                [TextType.PlainText] = new RenderInfo(TextType.PlainText, "", "")
            };
        }

        public HtmlRender(params RenderInfo[] renderInformation)
        {
            renderInfo =  new Dictionary<TextType, RenderInfo>();
            foreach (var info in renderInformation)
            {
                renderInfo[info.Type] = info;
            }
        }

        public HtmlRender(Dictionary<TextType, RenderInfo> renderInfo)
        {
            this.renderInfo = renderInfo;
        }

        public bool CanRender(TextType type)
        {
            return renderInfo.ContainsKey(type);
        }

        public string Render(HyperTextElement textGraph)
        {
            var render = new StringBuilder();
            Render(textGraph, render);
            return render.ToString().TrimEnd('\n');
        }
        
        private void Render(HyperTextElement element,  StringBuilder currentRender)
        {
            currentRender.Append(renderInfo[element.Type].Prefix);
            foreach (var child in element.Children)
            {
                if (renderInfo.ContainsKey(child.Type))
                    Render(child, currentRender);
                else
                    throw new ArgumentException("Can't render " + child.Type);
            }
            if (element is HyperTextElement<string> valueElement)
                currentRender.Append(valueElement.Value);
            currentRender.Append(renderInfo[element.Type].Suffix);
        }  
    }
}