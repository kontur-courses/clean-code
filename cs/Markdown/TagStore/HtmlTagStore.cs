using Markdown.Tags;

namespace Markdown.TagStore
{
    public class HtmlTagStore : TagStore
    {
        private static ITag[] tags =
        {
            new Tag("<em>", TagType.Emphasized),
            new Tag("</em>", TagType.Emphasized)
        };

        public HtmlTagStore()
        {
            foreach (var tag in tags)
            {
                Register(tag);
            }
        }
    }
}