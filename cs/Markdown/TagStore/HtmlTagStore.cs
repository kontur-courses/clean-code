using Markdown.Tags;

namespace Markdown.TagStore
{
    public class HtmlTagStore : TagStore
    {
        private static ITag[] tags =
        {
            new Tag(TagType.Emphasized, "<em>", "</em>"),
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