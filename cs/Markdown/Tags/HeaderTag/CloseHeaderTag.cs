namespace Markdown.Tags.HeaderTag
{
    public class CloseHeaderTag : HeaderTag
    {
        public CloseHeaderTag(int index) : base("</h1>", index, 1)
        {
        }
    }
}