namespace Markdown
{
    public class HTMLTagToken : MdToken, IHTMLTag
    {
        public string TagName { get; }
        public bool IsOpen { get; set; }

        protected HTMLTagToken(
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