using Rendering.Html.Abstract;

namespace Rendering.Html
{
    public static class Extensions
    {
        public static string OpenTag(this IMarkdownElementRenderer renderer) => $"<{renderer.TagText}>";
        public static string CloseTag(this IMarkdownElementRenderer renderer) => $"</{renderer.TagText}>";
    }
}