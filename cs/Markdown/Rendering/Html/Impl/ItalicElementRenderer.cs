using System.Text;
using MarkdownParser.Concrete.Italic;
using Rendering.Html.Abstract;

namespace Rendering.Html.Impl
{
    public class ItalicElementRenderer : MarkdownElementRenderer<MarkdownElementItalic>
    {
        public override string TagText { get; } = "em";
        public override string RenderElement(MarkdownElementItalic element)
        {
            var content = new StringBuilder();
            content.Append(this.OpenTag());

            foreach (var entry in element.Content)
                content.Append(entry.RawValue);

            content.Append(this.CloseTag());
            return content.ToString();
        }
    }
}