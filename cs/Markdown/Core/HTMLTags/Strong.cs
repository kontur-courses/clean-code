namespace Markdown
{
    public class Strong : HTMLTagToken
    {
        public Strong(int position, bool isOpen)
            : base(position, 2, "__", MdTokenType.HTMLTag, "strong", isOpen)
        {
        }
    }
}