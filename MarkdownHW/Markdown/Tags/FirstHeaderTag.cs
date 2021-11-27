namespace Markdown.Tokens
{
    public class FirstHeaderTag: Tag
    {
        public FirstHeaderTag(string text, int tagStartsAt) : base(text, tagStartsAt)
        {
        }

        protected override string MdTag => "#";
        protected override string HtmlTag => "h1";
    }
}