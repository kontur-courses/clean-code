namespace Markdown.Tokens
{
    public class StrongTag: Tag
    {
        public StrongTag(string text, int tagStartsAt) : base(text, tagStartsAt)
        {
        }

        protected override string MdTag => "__";
        protected override string HtmlTag => "strong";
    }
}