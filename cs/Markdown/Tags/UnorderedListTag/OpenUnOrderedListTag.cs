namespace Markdown.Tags.UnorderedListTag
{
    public class OpenUnOrderedListTag : UnorderedListTag
    {
        public OpenUnOrderedListTag(int index) : base("<ul>\n", index)
        {
        }
    }
}