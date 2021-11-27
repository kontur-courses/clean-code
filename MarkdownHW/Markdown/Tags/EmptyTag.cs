namespace Markdown.Tokens
{
    public class EmptyTag: Tag
    {
        public EmptyTag(string text, int tagStartsAt) : base(text, tagStartsAt)
        {
        }

        protected override string MdTag => "";
        protected override string HtmlTag => "";
        
        public override string GetHtml => text[startsAt..(endsAt+1)];
    }
}