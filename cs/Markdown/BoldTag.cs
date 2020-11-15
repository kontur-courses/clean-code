namespace Markdown
{
    public class BoldTag : Tag
    {
        public BoldTag(string value, int index) : base(value, index, 2)
        {
            
        }
    }
    
    public class OpenBoldTag : BoldTag
    {
        public OpenBoldTag( int index) : base("<strong>", index)
        {
        }
    }
    public class CloseBoldTag : BoldTag
    {
        public CloseBoldTag(int index) : base("</strong>", index)
        {
        }
    }
}