namespace Markdown
{
    public class OpenBoldTag : BoldTag
    {
        public OpenBoldTag( int index) : base("<strong>", index)
        {
        }
    }
}