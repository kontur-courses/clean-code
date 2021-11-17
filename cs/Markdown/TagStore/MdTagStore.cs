using Markdown.Tags;

namespace Markdown.TagStore
{
    public class MdTagStore : TagStore
    {
        private static ITag[] tags =
        {
            new Tag(TagType.Emphasized, "_", "_"),
        };

        public MdTagStore()
        {
            foreach (var tag in tags)
            {
                Register(tag);
            }
        }

        public override TagRole GetTagRole(string tag, char? before, char? after)
        {
            if (before != ' ' && before != null)
                return after is ' ' or null ? TagRole.Closing : TagRole.NotTag;
            return after != ' ' ? TagRole.Opening : TagRole.NotTag;
        }
    }
}