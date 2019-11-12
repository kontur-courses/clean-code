namespace Markdown
{
    public class Strong : IHTMLTagTokenToken
    {
        public Strong(int position, bool isOpen)
            : base(position, 2, "__", MdTokenType.HTMLTag, "strong", isOpen)
        {
        }
    }
}