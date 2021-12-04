using MarkdownTask.Tags;
using System.Text;

namespace MarkdownTask.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder WrapContentToTag(this StringBuilder builder, Tag tag)
        {
            return new StringBuilder(string.Join("", tag.OpeningPart, builder, tag.ClosingPart));
        }
    }
}
