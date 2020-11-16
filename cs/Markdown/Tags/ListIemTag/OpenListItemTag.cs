namespace Markdown.Tags.ListIemTag
{
    public class OpenListItemTag : ListItemTag
    {
        public OpenListItemTag(int index) : base("<li>", index, 2)
        {
        }
    }
}