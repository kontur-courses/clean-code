namespace Markdown.Tags
{
    public class SingleTag : Tag
    {
        public SingleTag(string mdTag, int position)
        {
            this.mdTag = mdTag;

            this.position = position;
        }
    }
}