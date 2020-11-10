using System;
using MarkdownParser.Infrastructure.Markdown.Abstract;

namespace HtmlMarkdownRenderer
{
    public interface IMarkdownElementRenderer
    {
        public Type ValidElementType { get; }
        public string RenderElement(MarkdownElement element);
    }
}