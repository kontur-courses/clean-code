namespace Markdown.Tags.ListIemTag
{
    public class CloseListItemTag: ListItemTag
    {
        public CloseListItemTag(int index) : base("</li>", index, 0)
        {
        }
    }
}