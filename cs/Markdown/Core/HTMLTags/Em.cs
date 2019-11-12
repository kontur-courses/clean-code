namespace Markdown
{
    public class Em : HTMLTagToken
    {
        public Em(int position, bool isOpen) 
            : base(position, 1, "_", MdTokenType.HTMLTag, "em", isOpen)
        {
        }
    }
}