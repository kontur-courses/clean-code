using Markdown.Tag;

namespace Markdown.Extensions
{
    public static class TagExtensions
    {
        public static string GetTextContent(this ITag tag, string text) =>
            text.Substring(tag.OpenIndex, tag.CloseIndex - tag.OpenIndex + 1);

        public static string GetTagContent(this ITag tag, string text) =>
            text.Substring(tag.OpenIndex + tag.Length, tag.CloseIndex - tag.OpenIndex - tag.Length);
    }
}