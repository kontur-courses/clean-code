namespace Markdown
{
    public class StrongTag : SelectingTag
    {
        public override string OpenHTMLTag => "<strong>";
        public override string CloseHTMLTag => "</strong>";
        public override string OpenMdTag => "__";
        public override string CloseMdTag => OpenMdTag;
        public override bool AllowNesting => true;
    }
}
