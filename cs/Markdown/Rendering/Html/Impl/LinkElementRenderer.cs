using MarkdownParser.Concrete.Link;
using Rendering.Html.Abstract;

namespace Rendering.Html.Impl
{
    public class LinkElementRenderer : MarkdownElementRenderer<MdElementLink>
    {
        public override string TagText { get; } = "a";

        public override string RenderElement(MdElementLink element)
        {
            var url = element.Tokens[0].RawValue;
            return $"<{TagText} href=\"{url}\">{url}</{TagText}>";
        }
    }
}