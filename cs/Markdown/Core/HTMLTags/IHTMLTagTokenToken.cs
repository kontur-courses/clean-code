namespace Markdown
{
    public class IHTMLTagTokenToken : MdToken, IHTMLTagToken
    {
        public string TagName { get; }
        public bool IsOpen { get; set; }

        protected IHTMLTagTokenToken(
            int position,
            int length,
            string value,
            MdTokenType tokenType,
            string tagName,
            bool isOpen)
            : base(position, length, value, tokenType)
        {
            TagName = tagName;
            IsOpen = isOpen;
        }
    }
}