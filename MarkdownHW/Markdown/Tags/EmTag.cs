namespace Markdown.Tokens
{
    public class EmTag: Tag
    {
        public EmTag(string text, int tagStartsAt) : base(text, tagStartsAt)
        {
        }

        protected override string MdTag => "_";
        protected override string HtmlTag => "em";
    }
}