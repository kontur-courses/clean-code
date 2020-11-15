namespace Markdown
{
    public class HeaderTag : Tag
    {
        public HeaderTag(string value, int index, int length) : base(value, index, length)
        {
        }
    }

    public class OpenHeaderTag : HeaderTag
    {
        public OpenHeaderTag( int index) : base("<h1>", index, 1)
        {
        }
    }

    public class CloseHeaderTag : HeaderTag
    {
        public CloseHeaderTag(int index) : base("</h1>", index, 1)
        {
        }
    }
}