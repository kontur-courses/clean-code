using Markdown.Attribute;
using Markdown.Tag;

namespace Markdown.Extensions
{
    public static class TagExtensions
    {
        public static string GetPairedTagContent(this ITag tag, string text) =>
            text.Substring(tag.OpenIndex + tag.Length, tag.CloseIndex - tag.OpenIndex - tag.Length);

        public static int FindClosePairedTagIndex(this ITag tag, string text)
        {
            for (var i = tag.OpenIndex + 2; i < text.Length - tag.Length + 1; i++)
            {
                var symbolAfterTag = text.LookAt(i + tag.Length);
                var symbolBeforeTag = text.LookAt(i - 1);

                if (text.Substring(i, tag.Length) == tag.Symbol && (char.IsWhiteSpace(symbolAfterTag) ||
                                                                    i == text.Length - tag.Length)
                                                                && char.IsLetter(symbolBeforeTag))
                    return i;
            }

            return -1;
        }

        public static IAttribute GetLinkTagAttribute(this ITag tag, string text)
        {
            if (tag is LinkTag linkTag)
            {
                var value = text.Substring(linkTag.MiddleIndex + linkTag.Length * 2,
                    linkTag.CloseIndex - linkTag.MiddleIndex - linkTag.Length * 2);
                return new LinkAttribute(value, "href");
            }

            return null;
        }
    }
}