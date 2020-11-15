namespace Markdown
{
    public class CloseBoldTag : BoldTag
    {
        public CloseBoldTag(int index) : base("</strong>", index)
        {
        }
    }
}