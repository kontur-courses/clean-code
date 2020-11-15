namespace Markdown
{
    public class CloseItalicTag : ItalicTag
    {
        public CloseItalicTag(int index) : base("</em>", index)
        {
        }
    }
}