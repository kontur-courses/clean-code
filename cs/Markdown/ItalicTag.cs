namespace Markdown
{
    public class ItalicTag : Tag
    {
        public ItalicTag(string value, int index) : base(value, index, 1)
        {
        }
    }

    public class OpenItalicTag : ItalicTag
    {
        public OpenItalicTag(int index) : base("<em>", index)
        {
        }
    }

    public class CloseItalicTag : ItalicTag
    {
        public CloseItalicTag(int index) : base("</em>", index)
        {
        }
    }
}