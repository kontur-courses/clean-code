using Markdown.Tags;
using Markdown.Tokens;

namespace Markdown.Extensions
{
    public static class TagExtensions
    {
        public static TagNode ToNode(this Tag tag) => new(tag);
    }
}