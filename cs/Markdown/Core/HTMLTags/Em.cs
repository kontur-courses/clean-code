namespace Markdown
{
    public class Em : IHTMLTagTokenToken
    {
        public Em(int position, bool isOpen) 
            : base(position, 1, "_", MdTokenType.HTMLTag, "em", isOpen)
        {
        }
    }
}