namespace Markdown.Tags.UnorderedListTag
{
    public class CloseUnOrderedListTag : UnorderedListTag
    {
        public CloseUnOrderedListTag(int index) : base("</ul>\n", index)
        {
        }
    }
}