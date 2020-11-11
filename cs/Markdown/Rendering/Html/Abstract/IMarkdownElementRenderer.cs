using System;
using MarkdownParser.Infrastructure.Markdown.Abstract;

namespace Rendering.Html.Abstract
{
    public interface IMarkdownElementRenderer
    {
        string TagText { get; }
        Type ValidElementType { get; }
        string RenderElement(MarkdownElement element);
    }
}